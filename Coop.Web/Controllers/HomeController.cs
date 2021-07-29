using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coop.Application.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coop.Web.Models;

namespace Coop.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int PAGE_SIZE = 10;
        
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public IActionResult Index(int page = 1)
        {
            return View(_newsService.GetPage(page,PAGE_SIZE));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}