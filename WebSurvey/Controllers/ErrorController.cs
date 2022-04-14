using Microsoft.AspNetCore.Mvc;

namespace WebSurvey.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CorruptSurvey()
        {
            return View();
        }

        public IActionResult NeedToSignIn(string returnUrl)
        {
            return View();
        }

        public IActionResult SurveyNotFound()
        {
            return View();
        }

        public IActionResult WrongPassword()
        {
            return View();
        }

        public IActionResult SurveyClosed()
        {
            return View();
        }

        public IActionResult AlreadyUsed()
        {
            return View();
        }
    }
}
