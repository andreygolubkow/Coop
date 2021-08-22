using System.Drawing;
using Microsoft.Extensions.Options;
using QRCoder;

namespace Coop.Application.QrPay
{
    public class QrPay : IQrPay
    {
        private readonly QrPayOptions _options;

        public QrPay(IOptions<QrPayOptions> options)
        {
            _options = options.Value;
        }

        public Bitmap GenerateCode(string paymentSubject, int sum)
        {
            var data = string.Format(_options.Data, paymentSubject, sum);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data,
                QRCodeGenerator.ECCLevel.M);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(30);
            return qrCodeImage;
        }
    }
}