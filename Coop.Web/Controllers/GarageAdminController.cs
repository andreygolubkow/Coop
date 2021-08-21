using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coop.Application.Common;
using Coop.Application.Realty;
using Coop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;

namespace Coop.Web.Controllers
{

    [Route("[controller]/[action]")] 
    [Authorize(Roles = Constants.ADMIN_ROLE)]
    public class GarageAdminController:Controller
    {
        public const int PageSize = 10;
        
        private readonly IRealtyService _realtyService;

        public GarageAdminController(IRealtyService realtyService)
        {
            _realtyService = realtyService;
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
                var errors = String.Join(" ", ModelState
                        .Where(e => e.Value.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(e => e.Value.Errors.Select(e =>e.ErrorMessage))
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
                    realtyId = realtyId
                });
            }
            catch (InvalidOperationException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId = realtyId
                });
            }
            catch (DatabaseException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId = realtyId
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
        public async Task<IActionResult> ConfirmChangeOwner([FromForm]Guid realtyId,[FromForm] Guid userId, CancellationToken token)
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
                    realtyId = realtyId
                });
            }
            catch (InvalidOperationException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId = realtyId
                });
            }
            catch (DatabaseException e)
            {
                return RedirectToAction("RequestRemoveRealty", new
                {
                    realtyId = realtyId
                });
            }
            return RedirectToAction("Realty");
        }

        [HttpGet]
        public IActionResult UploadPays()
        {
            return View();
        }
    }
}