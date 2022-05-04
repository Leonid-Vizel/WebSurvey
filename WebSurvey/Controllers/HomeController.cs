using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using WebSurvey.Data;
using WebSurvey.Models;
using WebSurvey.Models.Survey;

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
        public IActionResult Index(SearchModel model)
        {
            switch (model.Type)
            {
                case EntityType.Survey:
                    if (!ModelState.IsValid && ModelState["SearchSurvey"].ValidationState != ModelValidationState.Valid)
                    {
                        return View(model);
                    }
                    if (db.Surveys.Any(x => x.Id == model.SearchSurvey))
                    {
                        return RedirectToAction(controllerName: "Survey", actionName: "Status", routeValues: new { Id = model.SearchSurvey });
                    }
                    else
                    {
                        ModelState.AddModelError("SearchSurvey", "Опрос не найден");
                        return View(model);
                    }
                case EntityType.Voting:
                    if (!ModelState.IsValid && ModelState["SearchVoting"].ValidationState != ModelValidationState.Valid)
                    {
                        return View(model);
                    }
                    if (db.Votings.Any(x => x.Id == model.SearchVoting))
                    {
                        return RedirectToAction(controllerName: "Voting", actionName: "Status", routeValues: new { Id = model.SearchVoting });
                    }
                    else
                    {
                        ModelState.AddModelError("SearchVoting", "Голосование не найдено");
                        return View(model);
                    }
                default:
                    ModelState.AddModelError("SearchVoting", "Элемент не найден");
                    ModelState.AddModelError("SearchSurvey", "Элемент не найден");
                    return View(model);
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