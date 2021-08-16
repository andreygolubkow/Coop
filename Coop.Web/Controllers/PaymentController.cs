using System;
using System.Drawing;
using System.IO;
using Coop.Application.QrPay;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class PaymentController:Controller
    {
        private readonly IQrPay _qrPay;

        public PaymentController(IQrPay qrPay)
        {
            _qrPay = qrPay;
        }

        [ValidateDNTCaptcha(CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits, ErrorMessage = 
            "Повторите ввод проверочного кода")]
        [HttpGet()]
        public IActionResult GetPayImage([FromQuery]string subject,[FromQuery] int sum)
        {
            var code = _qrPay.GenerateCode(subject, sum);
            var bytes = BitmapToBytes(code);
            var formatted = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bytes)); 
            return Ok(formatted);
        }
        
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}