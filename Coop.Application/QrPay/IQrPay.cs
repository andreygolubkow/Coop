using System.Drawing;

namespace Coop.Application.QrPay
{
    public interface IQrPay
    {
        Bitmap GenerateCode(string paymentSubject, int sum);
    }
}