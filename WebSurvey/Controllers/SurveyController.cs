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

        public IActionResult Take(int Id)
        {
            Models.Database.Survey? foundSurvey = db.Surveys.FirstOrDefault(x=>x.Id == Id);
            if (foundSurvey != null)
            {
                List<Models.Database.SurveyQuestion> foundQuestions = db.Questions.Where(x => x.SurveyId == Id).ToList();
                if (foundQuestions.Count() > 0)
                {
                    Models.ViewModel.SurveyQuestion[] questionArray = new Models.ViewModel.SurveyQuestion[foundQuestions.Count()];
                    int counter = 0;
                    foreach (Models.Database.SurveyQuestion question in foundQuestions)
                    {
                        IEnumerable<SurveyQuestionOption> options = db.Options.Where(x => x.QuestionId == question.Id);
                        if (question.Type == QuestionType.Text || options.Count() > 0)
                        {
                            questionArray[counter++] = new Models.ViewModel.SurveyQuestion(question, options.ToArray());
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    Models.ViewModel.SurveyResult emptyResults = new Models.ViewModel.SurveyResult(Id, questionArray);
                    return View((emptyResults, foundSurvey.Name));
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
