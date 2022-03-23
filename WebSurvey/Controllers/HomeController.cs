using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSurvey.Data;
using WebSurvey.Models;

namespace WebSurvey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SurveySearchModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Surveys.Any(x=>x.Id == model.Search))
                {
                    return RedirectToAction(controllerName: "Survey", actionName: "Status", routeValues: new { Id = model.Search } );
                }
                else
                {
                    ModelState.AddModelError("Search","Опрос не найден");
                    return View(model);
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}