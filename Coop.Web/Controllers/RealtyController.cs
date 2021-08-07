using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    public class RealtyController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }
    }
}