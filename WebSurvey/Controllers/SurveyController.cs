using Microsoft.AspNetCore.Mvc;
using WebSurvey.Data;
using WebSurvey.Models;
using WebSurvey.Models.Database;
using WebSurvey.Models.ViewModel;

namespace WebSurvey.Controllers
{
    public class SurveyController : Controller
    {
        private ApplicationDbContext db;
        public SurveyController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int Id)
        {
            return View();
        }

        public IActionResult Status(int Id, string Password)
        {
            Models.Database.Survey? foundSurvey = db.Surveys.FirstOrDefault(s => s.Id == Id);
            if (foundSurvey != null)
            {
                //if (foundSurvey.IsPassworded)
                //{
                //    if (!foundSurvey.Password.Equals(Password))
                //    {
                //        return NotFound();
                //        //Error message
                //    }
                //}
                //if (!foundSurvey.IsAnonimous || foundSurvey.IsOneOff)
                //{
                //    if (true /*Check logged in*/)
                //    {

                //    }
                //    if (foundSurvey.IsOneOff)
                //    {
                //        //check if 
                //    }
                //}
                return View(new SurveyStatistics(foundSurvey, db.Results.Count(x => x.Id == Id)));
            }
            else
            {
                return NotFound();
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

        public IActionResult Take(int SurveyId)
        {
            Models.Database.Survey? foundSurvey = db.Surveys.FirstOrDefault(x=>x.Id == SurveyId);
            if (foundSurvey != null)
            {
                List<Models.Database.SurveyQuestion> foundQuestions = db.Questions.Where(x => x.SurveyId == SurveyId).ToList();
                if (foundQuestions.Count() > 0)
                {
                    List<Models.ViewModel.SurveyQuestion> questionList = new List<Models.ViewModel.SurveyQuestion>(foundQuestions.Count());
                    foreach (Models.Database.SurveyQuestion question in foundQuestions)
                    {
                        IEnumerable<SurveyQuestionOption> options = db.Options.Where(x => x.QuestionId == question.Id);
                        if (question.Type == QuestionType.Text || options.Count() > 0)
                        {
                            questionList.Add(new Models.ViewModel.SurveyQuestion(question, options.ToArray()));
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    Models.ViewModel.SurveyResult emptyResults = new Models.ViewModel.SurveyResult(foundSurvey, questionList);
                    return View(emptyResults);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Take")]
        public IActionResult TakePOST(Models.ViewModel.SurveyResult res)
        {
            db.Results.Add(res.ToDbClass());
            db.SaveChanges();
            return NotFound();
        }

        public IActionResult Close(int Id)
        {
            //Только авторизованный и автор
            return View();
        }

        public IActionResult Delete(int Id)
        {
            //Только авторизованный и автор
            return View();
        }

        public IActionResult Results(int Id)
        {
            //Только авторизованный и автор
            return View();
        }
    }
}
