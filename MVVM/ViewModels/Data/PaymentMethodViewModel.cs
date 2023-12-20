using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class PaymentMethodViewModel
    {
        private readonly PaymentMethod _paymentMethod;
        public PaymentMethodViewModel(PaymentMethod PayMet)
        {
            _paymentMethod = PayMet;
        }
        public static async Task<List<PaymentMethodViewModel>> GetMethods() => await PaymentMethod.GetMethods();

        public int Id => _paymentMethod.Id;

        public string Method => _paymentMethod.Method;
    }
}
