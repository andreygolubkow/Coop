using System.Threading.Tasks;
using Coop.Application.Realty;
using Coop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class RealtyController : Controller
    {
        private readonly IRealtyService _realtyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RealtyController(IRealtyService realtyService, UserManager<ApplicationUser> userManager)
        {
            _realtyService = realtyService;
            _userManager = userManager;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return View(_realtyService.GetForUser(user.Id));
        }

        public IActionResult History()
        {
            return View();
        }
    }
}