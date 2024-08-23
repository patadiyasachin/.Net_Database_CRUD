using Microsoft.AspNetCore.Mvc;

namespace Admin3.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult BlankPage()
        {
            return View();
        }
    }
}
