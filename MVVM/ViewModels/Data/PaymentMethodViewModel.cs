using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class PaymentMethodViewModel(PaymentMethod PayMet)
    {
        private readonly PaymentMethod _paymentMethod = PayMet;

        public static async Task<List<PaymentMethodViewModel>> GetMethods() => await PaymentMethod.GetMethods();

        public int Id => _paymentMethod.Id;

        public string Method => _paymentMethod.Method;
    }
}
