using Microsoft.AspNetCore.Mvc;

namespace TechnoWave.UI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
