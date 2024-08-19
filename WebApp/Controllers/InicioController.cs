using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
