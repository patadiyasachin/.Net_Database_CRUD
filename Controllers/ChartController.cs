using Microsoft.AspNetCore.Mvc;

namespace Admin3.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult ChartJs()
        {
            return View();
        }
        public IActionResult ApexCharts()
        {
            return View();
        }
        public IActionResult ECharts()
        {
            return View();
        }
    }
}
