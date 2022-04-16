using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSurvey.Data;
using WebSurvey.Models;
using WebSurvey.Models.Database;
using WebSurvey.Models.ViewModel;

namespace WebSurvey.Controllers
{
    public class SurveyController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private ApplicationDbContext db;
        public SurveyController(ApplicationDbContext db, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            if (signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSurveyModel builtSurvey)
        {
            if (signInManager.IsSignedIn(User))
            {
                if (builtSurvey.Questions.Length > 0)
                {
                    if (builtSurvey.Questions.All(x => x.Type == QuestionType.Text || (x.Options != null && x.Options.Count() > 0)))
                    {
                        builtSurvey.AuthorId = userManager.GetUserId(User);
                        builtSurvey.CreatedDate = DateTime.Now;
                        builtSurvey.IsClosed = false;
                        await db.Surveys.AddAsync(builtSurvey);
                        await db.SaveChangesAsync(); //For Id population
                        foreach (Models.ViewModel.SurveyQuestion question in builtSurvey.Questions)
                        {
                            question.SurveyId = builtSurvey.Id;
                        }
                        await db.SurveyQuestions.AddRangeAsync(builtSurvey.Questions);
                        await db.SaveChangesAsync(); //For Id population
                        foreach (Models.ViewModel.SurveyQuestion question in builtSurvey.Questions)
                        {
                            if (question.Options != null)
                            {
                                foreach (SurveyQuestionOption option in question.Options)
                                {
                                    option.QuestionId = question.Id;
                                }
                                await db.SurveyQuestionOptions.AddRangeAsync(question.Options);
                            }
                        }
                        await db.SaveChangesAsync();
                        return RedirectToAction("Status", new { Id = builtSurvey.Id });
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "CorruptSurvey");
                    }
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "CorruptSurvey");
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        public IActionResult Status(int Id)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(s => s.Id == Id);
            if (foundSurvey != null)
            {
                return View(new SurveyStatistics(foundSurvey, db.SurveyResults.Count(x => x.SurveyId == Id)));
            }
            else
            {
                return RedirectToAction("SurveyNotFound","Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Status(SurveyStatistics passwordInfo)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(s => s.Id == passwordInfo.Id);
            if (foundSurvey != null)
            {
                if (foundSurvey.Password == null)
                {
                    return RedirectToAction("Take", new { SurveyId = passwordInfo.Id});
                }
                else
                {
                    if (foundSurvey.Password.Equals(passwordInfo.Password))
                    {
                        return RedirectToAction("Take", new { SurveyId = passwordInfo.Id, password = passwordInfo.Password });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Неверный пароль");
                        SurveyStatistics newStatistics = new SurveyStatistics(foundSurvey, db.SurveyResults.Count(x => x.SurveyId == foundSurvey.Id));
                        newStatistics.Password = passwordInfo.Password;
                        return View(newStatistics);
                    }
                }
            }
            else
            {
                return RedirectToAction("SurveyNotFound", "Error");
            }
        }

        public IActionResult Select()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Select(SurveySearchModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Surveys.Any(x => x.Id == model.Search))
                {
                    return RedirectToAction("Status", new { Id = model.Search });
                }
                else
                {
                    ModelState.AddModelError("Search", "Опрос не найден");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Take(int SurveyId, string password)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(x => x.Id == SurveyId);
            if (foundSurvey != null)
            {
                if (foundSurvey.IsClosed)
                {
                    return RedirectToAction("SurveyClosed", "Error");
                }
                if (!foundSurvey.IsAnonimous || foundSurvey.IsOneOff)
                {
                    if (signInManager.IsSignedIn(User))
                    {
                        if (foundSurvey.IsOneOff && !db.SurveyResults.Any(x => x.SurveyId == foundSurvey.Id && x.UserId.Equals(userManager.GetUserId(User))))
                        {
                            return RedirectToAction("AlreadyUsed", "Error");
                        }
                    }
                    else
                    {
                        return RedirectToAction("NeedToSignIn", "Error");
                    }
                }
                if (foundSurvey.IsPassworded && foundSurvey.Password != null && !foundSurvey.Password.Equals(password))
                {
                    return RedirectToAction("WrongPassword", "Error");
                }
                List<Models.Database.SurveyQuestion> foundQuestions = db.SurveyQuestions.Where(x => x.SurveyId == SurveyId).ToList();
                if (foundQuestions.Count() > 0)
                {
                    List<Models.ViewModel.SurveyQuestion> questionList = new List<Models.ViewModel.SurveyQuestion>(foundQuestions.Count());
                    foreach (Models.Database.SurveyQuestion question in foundQuestions)
                    {
                        IEnumerable<SurveyQuestionOption> options = db.SurveyQuestionOptions.Where(x => x.QuestionId == question.Id);
                        if (question.Type == QuestionType.Text || options.Count() > 0)
                        {
                            questionList.Add(new Models.ViewModel.SurveyQuestion(question, options.ToArray()));
                        }
                        else
                        {
                            return RedirectToAction("CorruptSurvey", "Error");
                        }
                    }
                    Models.ViewModel.SurveyResult emptyResults = new Models.ViewModel.SurveyResult(foundSurvey, questionList);
                    return View(emptyResults);
                }
                else
                {
                    return RedirectToAction("CorruptSurvey", "Error");
                }
            }
            else
            {
                return RedirectToAction("SurveyNotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Take")]
        public IActionResult TakePOST(Models.ViewModel.SurveyResult res)
        {
            db.SurveyResults.Add(res.ToDbClass());
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Close(int Id)
        {
            //Только авторизованный и автор
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Close")]
        public async Task<IActionResult> ClosePOST(int Id)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(x => x.Id == Id);
            if (foundSurvey != null)
            {
                foundSurvey.IsClosed = true;
                await db.Surveys.AddAsync(foundSurvey);
                await db.SaveChangesAsync();
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
            else
            {
                return RedirectToAction("SurveyNotFound", "Error");
            }
        }

        public IActionResult Delete(int Id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int Id)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(x => x.Id == Id);
            if (foundSurvey != null)
            {
                db.Surveys.Remove(foundSurvey);
                Models.Database.SurveyQuestion[] arrayToDelete = db.SurveyQuestions.Where(x => x.SurveyId == foundSurvey.Id).ToArray();
                foreach (Models.Database.SurveyQuestion question in arrayToDelete)
                {
                    db.RemoveRange(db.SurveyQuestionOptions.Where(x => x.QuestionId == question.Id));
                }
                db.RemoveRange(arrayToDelete);
                db.RemoveRange(db.SurveyResults.Where(x => x.SurveyId == foundSurvey.Id));
                await db.SaveChangesAsync();
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
            else
            {
                return RedirectToAction("SurveyNotFound", "Error");
            }
        }

        public IActionResult Results(int Id)
        {
            //Только авторизованный и автор
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Results")]
        public IActionResult ResultsPOST(int Id)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(x => x.Id == Id);
            if (foundSurvey != null)
            {
                List<Models.ViewModel.SurveyResult> clearResults = new List<Models.ViewModel.SurveyResult>();
                List<Models.ViewModel.SurveyQuestion> clearSurveyQuestions = new List<Models.ViewModel.SurveyQuestion>();
                foreach (Models.Database.SurveyQuestion question in db.SurveyQuestions.Where(x => x.SurveyId == foundSurvey.Id))
                {
                    clearSurveyQuestions.Add(new Models.ViewModel.SurveyQuestion(question, null));
                }
                foreach (Models.Database.SurveyResult result in db.SurveyResults.Where(x => x.SurveyId == foundSurvey.Id))
                {
                    clearResults.Add(new Models.ViewModel.SurveyResult(result, foundSurvey.Name, clearSurveyQuestions));
                }

                using (XLWorkbook workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Results");
                    int currentRow = 1;
                    int currentCol = 1;
                    worksheet.Cell(currentRow, currentCol++).Value = "Id";
                    if (!foundSurvey.IsAnonimous)
                    {
                        worksheet.Cell(currentRow, currentCol++).Value = "Никнейм";
                    }

                    foreach (Models.ViewModel.SurveyQuestion questionHeader in clearSurveyQuestions)
                    {
                        worksheet.Cell(currentRow, currentCol++).Value = questionHeader.Name;
                    }
                    foreach (Models.ViewModel.SurveyResult result in clearResults)
                    {
                        currentCol = 1;
                        worksheet.Cell(++currentRow, currentCol++).Value = result.Id;
                        if (!foundSurvey.IsAnonimous)
                        {
                            worksheet.Cell(currentRow, currentCol++).Value = result.UserId;
                        }
                        for (int i = 0; i < result.Results.Count; i++)
                        {
                            if (result.Questions[i].Type == QuestionType.Check)
                            {
                                worksheet.Cell(currentRow, currentCol++).Value = string.Join("; ", result.Results[i].CheckAnswers);
                            }
                            else
                            {
                                worksheet.Cell(currentRow, currentCol++).Value = result.Results[i].TextAnswer;
                            }
                        }
                    }

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        workbook.SaveAs(memStream);
                        byte[] content = memStream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "users.xlsx");
                    }
                }
            }
            else
            {
                return RedirectToAction("SurveyNotFound", "Error");
            }
        }
    }
}
