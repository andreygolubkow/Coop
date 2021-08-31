using System;
using System.Threading;
using System.Threading.Tasks;
using Coop.Application.Advertisement;
using Coop.Application.Common;
using Coop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    [NonController]
    public class AdvertisementController : Controller
    {
        public const int PAGE_SIZE = 10;
        private readonly IAdvertisementService _advertisementService;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdvertisementController(UserManager<ApplicationUser> userManager,
            IAdvertisementService advertisementService)
        {
            _userManager = userManager;
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            return View(_advertisementService.GetPage(page, PAGE_SIZE));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyAd(int page = 1, CancellationToken token = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Ваш профиль не найден. Попробуйте выйти и зайти на сайт.");
            return View(_advertisementService.GetForUser(user.Id, page, PAGE_SIZE));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateAdvertisementInputModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertisementInputModel model, CancellationToken token)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) model.AuthorId = user.Id;

            if (!ModelState.IsValid) return View(model);

            try
            {
                await _advertisementService.CreateAdvertisementAsync(model, token);
                return RedirectToAction("MyAd");
            }
            catch (DatabaseException)
            {
                ModelState.AddModelError("", "Произошла ошибка. Объявление не добавлено");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public IActionResult Archive(Guid id)
        {
            return View(id);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ConfirmArchive(Guid id, CancellationToken token)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Пользователь не идентифицирован. Попробуйте выйти из аккаунта и повторно войти");
            if (!User.IsInRole(Constants.ADMIN_ROLE) && _advertisementService.GetOwnerId(id) != user.Id)
                return Forbid("У вас нет прав на выполнение данного действия");
            try
            {
                await _advertisementService.ArchiveAsync(id, token);
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

            return RedirectToAction("Index", "Advertisement");
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet]
        public IActionResult NewAds(int page = 1)
        {
            var model = _advertisementService.GetNewAdvertisements(page, PAGE_SIZE);

            return View(model);
        }

        [Authorize(Roles = Constants.ADMIN_ROLE)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Publish(Guid id, string returnUrl = null, CancellationToken token = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                await _advertisementService.PublishAsync(id, user.Id, token);
            }
            catch (DatabaseException e)
            {
                return Problem("Не удалось опубликовать данное объявление. Обратитесь к администратору");
            }
            catch (InvalidOperationException)
            {
                //ignore
            }
            catch (ArgumentException)
            {
                //ignore
            }

            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("NewAds");
        }
    }
}