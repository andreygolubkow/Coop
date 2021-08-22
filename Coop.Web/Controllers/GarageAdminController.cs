using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coop.Application.Common;
using Coop.Application.Realty;
using Coop.Application.RealtyOwner;
using Coop.Web.Data;
using Coop.Web.DebtsParser;
using Coop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = Constants.ADMIN_ROLE)]
    public class GarageAdminController : Controller
    {
        public const int PageSize = 10;
        private readonly IRealtyOwnerService _realtyOwnerService;

        private readonly IRealtyService _realtyService;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IDebtParser _debtParser;

        public GarageAdminController(IRealtyService realtyService, IRealtyOwnerService realtyOwnerService,
            IUserStore<ApplicationUser> userStore, IDebtParser debtParser)
        {
            _realtyService = realtyService;
            _realtyOwnerService = realtyOwnerService;
            _userStore = userStore;
            _debtParser = debtParser;
        }

        [HttpGet]
        public IActionResult Realty()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRealtyPage(int page)
        {
            var model = _realtyService.GetPage(PageSize, page);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRealty(NewRealtyInputModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" ", ModelState
                    .Where(e => e.Value.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList());
                return BadRequest(errors);
            }

            try
            {
                await _realtyService.AddRealty(model, token);
                return Ok($"Объект {model.InventoryNumber} добавлен");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DatabaseException e)
            {
                return base.Problem("Произошла внутрення ошибка сервера, объект не добавлен");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RequestRemoveRealty(Guid realtyId, CancellationToken token)
        {
            var model = await _realtyService.GetFullInfoAsync(realtyId, token);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRemoveRealty([FromForm] Guid realtyId, CancellationToken token)
        {
            if (realtyId == Guid.Empty) return RedirectToAction("Realty");
            try
            {
                await _realtyService.Archive(realtyId, token);
            }
            catch (ArgumentException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }
            catch (InvalidOperationException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }
            catch (DatabaseException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }

            return RedirectToAction("Realty");
        }

        [HttpGet]
        public async Task<IActionResult> RequestChangeOwner(Guid realtyId, CancellationToken token)
        {
            var model = await _realtyService.GetFullInfoAsync(realtyId, token);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmChangeOwner([FromForm] Guid realtyId, [FromForm] Guid userId,
            CancellationToken token)
        {
            if (realtyId == Guid.Empty) return RedirectToAction("Realty");
            if (userId == Guid.Empty) return RedirectToAction("Realty");
            try
            {
                await _realtyService.SetOwner(realtyId, userId, token);
            }
            catch (ArgumentException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }
            catch (InvalidOperationException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }
            catch (DatabaseException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId
                });
            }

            return RedirectToAction("Realty");
        }

        [HttpGet]
        public IActionResult UploadPays()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadDebts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDebts([FromForm] IFormFile file, CancellationToken token)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, token);
            var viewModel = new UploadDebtViewModel();
            List<DebtRecord> debts = null;
            try
            {
                debts=_debtParser.ParseFromStream(memoryStream);
            }
            catch (Exception e)
            {
                viewModel.ParserError = $"{e.Message}";
                return View(viewModel);
            }

            foreach (var debtRecord in debts)
            {
                try
                {
                    var realty = _realtyService.FindActiveIdByName(debtRecord.InventoryNumber);
                    if (realty == null) continue;
                    await _realtyService.SetDebt(realty.Value, debtRecord.Amount, token);
                    viewModel.Result.Add($"Для {debtRecord.InventoryNumber} установлено {debtRecord.Amount}");
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RealtyFullInfo(Guid realtyId, CancellationToken token)
        {
            var owners = _realtyOwnerService.GetOwnersHistory(realtyId);
            var users = owners
                .Select(o => o.OwnerId)
                .Distinct()
                .ToDictionary(o => o,
                    o => string.Empty);
            foreach (var user in users)
            {
                var userAccount = await _userStore.FindByIdAsync(user.Key.ToString(), token);
                users[user.Key] = userAccount.FullName ?? userAccount.UserName;
            }

            return View(new RealtyFullPageViewModel
            {
                Realty = await _realtyService.GetFullInfoAsync(realtyId, token),
                OwnHistory = owners,
                Users = users
            });
        }
    }
}