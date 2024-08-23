using Microsoft.AspNetCore.Mvc;

namespace Admin3.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddEditEmployee()
        {
            return View();
        }
    }
}
