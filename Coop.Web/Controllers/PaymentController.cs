using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Coop.Application.QrPay;
using Coop.Web.Data;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IQrPay _qrPay;
        private readonly UserManager<ApplicationUser> _userManager;


        public PaymentController(IQrPay qrPay, UserManager<ApplicationUser> userManager)
        {
            _qrPay = qrPay;
            _userManager = userManager;
        }

        [ValidateDNTCaptcha(CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits, ErrorMessage =
            "Повторите ввод проверочного кода")]
        [HttpGet]
        public async Task<IActionResult> GetPayImage([FromQuery] string subject, [FromQuery] int sum,[FromQuery] string name)
        {
            
            var code = _qrPay.GenerateCode($"{name} {subject}", sum);
            var bytes = BitmapToBytes(code);
            var formatted = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bytes));
            return Ok(formatted);
        }
        
        

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}