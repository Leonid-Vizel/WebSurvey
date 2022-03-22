using Microsoft.AspNetCore.Mvc;

namespace WebSurvey.Controllers
{
    public class SurveyController : Controller
    {

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

            return View();
        }

        public IActionResult Take(int Id)
        {
            //Любой
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
