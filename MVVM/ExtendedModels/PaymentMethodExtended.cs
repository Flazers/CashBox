using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class PaymentMethod
    {
        private PaymentMethod() { }
        private static async Task<List<PaymentMethodViewModel>> CreateBasePayMet()
        {
            List<PaymentMethod> paymet = [];
            try
            {
                PaymentMethod card = new() { Id = 1, Method = "Карта" };
                PaymentMethod papper = new() { Id = 2, Method = "Наличные" };
                PaymentMethod trans = new() { Id = 3, Method = "Перевод" };

                paymet.Add(card);
                paymet.Add(papper);
                paymet.Add(trans);

                CashBoxDataContext.Context.PaymentMethods.AddRange(paymet);
                await CashBoxDataContext.Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return [];
            }
            return paymet.Select(s => new PaymentMethodViewModel(s)).ToList();
        }

        public static async Task<List<PaymentMethodViewModel>> GetMethods()
        {
            List<PaymentMethodViewModel> paymet;

            if (!CashBoxDataContext.Context.PaymentMethods.Any())
                paymet = await CreateBasePayMet();
            else
                paymet = await CashBoxDataContext.Context.PaymentMethods.Select(s => new PaymentMethodViewModel(s)).ToListAsync();

            return paymet;
        }
    }
}
