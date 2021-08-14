using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{

    [Authorize(Roles = Constants.ADMIN_ROLE)]
    public class AdminController:Controller
    {
        public AdminController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}