using Microsoft.AspNetCore.Mvc;

namespace WebSurvey.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CorruptSurvey()
        {
            return View();
        }

        public IActionResult CorruptVoting()
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
        public IActionResult VotingNotFound()
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

        public IActionResult VotingClosed()
        {
            return View();
        }

        public IActionResult SurveyUsed()
        {
            return View();
        }

        public IActionResult VotingUsed()
        {
            return View();
        }

        public IActionResult NotEnoughResults()
        {
            return View();
        }
    }
}
