using Microsoft.AspNetCore.Mvc;
using WebSurvey.Data;
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
            //Только авторизованный
            return View();
        }

        public IActionResult Edit(int Id)
        {
            //Только авторизованный и автор
            return View();
        }

        public IActionResult Status(int Id)
        {
            Models.Database.Survey? foundSurvey = db.Surveys.FirstOrDefault(s => s.Id == Id);
            if (foundSurvey != null)
            {
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

        public IActionResult Take(int Id)
        {
            return View();
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
