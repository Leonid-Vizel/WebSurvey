﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSurvey.Data;
using WebSurvey.Models.Survey;

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

        #region Create
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
        public async Task<IActionResult> Create(CreateSurveyModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsAnonimous && model.IsOneOff)
                {
                    ModelState.AddModelError("IsOneOff", "Опрос не может одновременно быть одноразовым и анонимным");
                    return View(model);
                }
                if (model.IsPassworded && model.Password == null)
                {
                    ModelState.AddModelError("Password", "Укажите пароль");
                    return View(model);
                }
                int errorCount = 0;
                for (int i = 0; i < model.Questions.Length; i++)
                {
                    if (model.Questions[i].Type != QuestionType.Text)
                    {
                        if (model.Questions[i].Options == null || model.Questions[i].Options.Length == 0)
                        {
                            ModelState.AddModelError($"Questions[{i}].Name", "У вопроса с флажками или радио-кнопками должна быть как минимум 1 опция");
                            errorCount++;
                        }
                        else
                        {
                            for (int j = 0; j < model.Questions[i].Options.Length; j++)
                            {
                                if (model.Questions[i].Options[j].Text == null)
                                {
                                    ModelState.AddModelError($"Questions[{i}].Options[{j}].Text", "Укажите опцию или удалите её");
                                    errorCount++;
                                }
                            }
                            if (errorCount > 0)
                            {
                                return View(model);
                            }
                            if (model.Questions[i].Options.Select(x => x.Text).Distinct().Count() != model.Questions[i].Options.Length)
                            {
                                ModelState.AddModelError($"Questions[{i}].Options", "Названия опций не могут повторятся");
                                return View(model);
                            }
                        }
                    }
                }
                if (errorCount > 0)
                {
                    return View(model);
                }
                if (signInManager.IsSignedIn(User))
                {
                    if (model.Questions.Length > 0)
                    {
                        if (model.Questions.All(x => x.Type == QuestionType.Text || (x.Options != null && x.Options.Count() > 0)))
                        {
                            model.AuthorId = userManager.GetUserId(User);
                            model.CreatedDate = DateTime.Now;
                            model.IsClosed = false;
                            await db.Surveys.AddAsync(model);
                            await db.SaveChangesAsync(); //For Id population
                            foreach (SurveyQuestion question in model.Questions)
                            {
                                question.SurveyId = model.Id;
                            }
                            await db.SurveyQuestions.AddRangeAsync(model.Questions);
                            await db.SaveChangesAsync(); //For Id population
                            foreach (SurveyQuestion question in model.Questions)
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
                            return RedirectToAction("Status", new { Id = model.Id });
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
            else
            {
                return View(model);
            }
        }
        #endregion

        #region Status
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
        #endregion

        #region Select
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
        #endregion

        #region Take
        public IActionResult Take(int SurveyId, string Password)
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
                if (foundSurvey.IsPassworded && foundSurvey.Password != null && !foundSurvey.Password.Equals(Password))
                {
                    return RedirectToAction("WrongPassword", "Error");
                }
                List<SurveyDbQuestion> foundQuestions = db.SurveyQuestions.Where(x => x.SurveyId == SurveyId).ToList();
                if (foundQuestions.Count() > 0)
                {
                    List<SurveyQuestion> questionList = new List<SurveyQuestion>(foundQuestions.Count());
                    foreach (SurveyDbQuestion question in foundQuestions)
                    {
                        IEnumerable<SurveyQuestionOption> options = db.SurveyQuestionOptions.Where(x => x.QuestionId == question.Id);
                        if (question.Type == QuestionType.Text || options.Count() > 0)
                        {
                            questionList.Add(new SurveyQuestion(question, options.ToArray()));
                        }
                        else
                        {
                            return RedirectToAction("CorruptSurvey", "Error");
                        }
                    }
                    SurveyResult emptyResults = new SurveyResult(foundSurvey, questionList);
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
        public IActionResult TakePOST(SurveyResult result)
        {
            db.SurveyResults.Add(result.ToDbClass());
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Close
        public IActionResult Close(int Id)
        {
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
        #endregion

        #region Delete
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
                SurveyDbQuestion[] arrayToDelete = db.SurveyQuestions.Where(x => x.SurveyId == foundSurvey.Id).ToArray();
                foreach (SurveyDbQuestion question in arrayToDelete)
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
        #endregion

        #region MySurveys

        public IActionResult MySurveys()
        {
            if (signInManager.IsSignedIn(User))
            {
                List<Survey> userSurveys = db.Surveys.Where(x => x.AuthorId.Equals(userManager.GetUserId(User))).ToList();
                List<SurveyStatistics> userSurveysAndStats = new List<SurveyStatistics>();
                foreach (Survey survey in userSurveys)
                {
                    userSurveysAndStats.Add(new SurveyStatistics(survey, db.SurveyResults.Count(x=>x.SurveyId == survey.Id), db.SurveyQuestions.Count(x=>x.SurveyId == survey.Id)));
                }
                return View(userSurveysAndStats);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MySurveys(int Id)
        {
            Survey? foundSurvey = db.Surveys.FirstOrDefault(x => x.Id == Id);
            if (foundSurvey != null)
            {
                List<SurveyResult> clearResults = new List<SurveyResult>();
                List<SurveyQuestion> clearSurveyQuestions = new List<SurveyQuestion>();
                foreach (SurveyDbQuestion question in db.SurveyQuestions.Where(x => x.SurveyId == foundSurvey.Id))
                {
                    clearSurveyQuestions.Add(new SurveyQuestion(question, null));
                }
                foreach (SurveyDbResult result in db.SurveyResults.Where(x => x.SurveyId == foundSurvey.Id))
                {
                    clearResults.Add(new SurveyResult(result, foundSurvey.Name, clearSurveyQuestions));
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

                    foreach (SurveyQuestion questionHeader in clearSurveyQuestions)
                    {
                        worksheet.Cell(currentRow, currentCol++).Value = questionHeader.Name;
                    }
                    foreach (SurveyResult result in clearResults)
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
        #endregion
    }
}
