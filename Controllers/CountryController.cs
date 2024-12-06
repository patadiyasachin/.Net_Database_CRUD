using Microsoft.AspNetCore.Mvc;

namespace Admin3.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
