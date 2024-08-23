using Microsoft.AspNetCore.Mvc;

namespace Admin3.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult GenrealTables()
        {
            return View();
        }
        public IActionResult DataTables()
        {
            return View();
        }
    }
}
