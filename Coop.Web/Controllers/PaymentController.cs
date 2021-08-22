using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Coop.Application.QrPay;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IQrPay _qrPay;

        public PaymentController(IQrPay qrPay)
        {
            _qrPay = qrPay;
        }

        [ValidateDNTCaptcha(CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits, ErrorMessage =
            "Повторите ввод проверочного кода")]
        [HttpGet]
        public IActionResult GetPayImage([FromQuery] string subject, [FromQuery] int sum)
        {
            var code = _qrPay.GenerateCode(subject, sum);
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