using System.Diagnostics;
using Coop.Application.Articles;
using Coop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coop.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 10;
        private readonly IArticleService _articleService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        public IActionResult Index(int page = 1)
        {
            return View(_articleService.GetPage(page, PageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}