using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    public class AdvertisementController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}