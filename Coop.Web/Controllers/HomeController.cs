using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coop.Application.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coop.Web.Models;

namespace Coop.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 10;
        
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        public IActionResult Index(int page = 1)
        {
            return View(_articleService.GetPage(page,PageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}