using System;
using System.Threading;
using System.Threading.Tasks;
using Coop.Application.Articles;
using Coop.Application.Common;
using Coop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IArticleService articleService, UserManager<ApplicationUser> userManager)
        {
            _articleService = articleService;
            _userManager = userManager;
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel model, CancellationToken token)
        {
            if (model == null) ModelState.AddModelError("", "Введите информацию для публикации");

            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.GetUserAsync(User);
            try
            {
                await _articleService.Create(model, user.Id, token);
                return RedirectToAction("Index", "Home");
            }
            catch (DatabaseException)
            {
                ModelState.AddModelError("", "Не удалось сохранить новость, обратитесь к администратору");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet("{id:guid}")]
        public IActionResult Update(Guid id)
        {
            var article = _articleService.Get(id);
            if (article == null) return NotFound("Такая новость не найдена");

            return View(article);
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpPost("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateArticleInputModel model,
            CancellationToken token)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _articleService.UpdateAsync(model, token);
                return RedirectToAction("Index", "Home");
            }
            catch (DatabaseException e)
            {
                ModelState.AddModelError("", "Не удалось сохранить новость");
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet("{id:guid}")]
        public IActionResult Archive(Guid id)
        {
            return View(id);
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ConfirmArchive(Guid id, CancellationToken token)
        {
            try
            {
                await _articleService.ArchiveAsync(id, token);
            }
            catch (DatabaseException e)
            {
                return View(model: "Не удалось удалить новость");
            }
            catch (InvalidOperationException e)
            {
                return View(model: e.Message);
            }
            catch (ArgumentException e)
            {
                return View(model: e.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}