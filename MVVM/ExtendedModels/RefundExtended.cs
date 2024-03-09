using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class Refund
    {
        private Refund() { }
        
        public static Refund? CurrentRefund { get; set; }

        public static async Task<RefundViewModel> CreateRefund()
        {
            Refund refund = new()
            {
                IsPurchased = false,
                IsSuccessRefund = false,
            };
            CashBoxDataContext.Context.Refunds.Add(refund);
            await CashBoxDataContext.Context.SaveChangesAsync();
            CurrentRefund = refund;
            return new(refund);
        }

        public static async Task<RefundViewModel> RemoveCurrentRefund()
        {
            CashBoxDataContext.Context.Refunds.Remove(CurrentRefund!);
            await CashBoxDataContext.Context.SaveChangesAsync();
            CurrentRefund = null;
            return new(CurrentRefund);
        }

        public static async Task<RefundViewModel> CreateRefundReason(string reason, DateOnly buydate, int productid)
        {
            CurrentRefund.BuyDate = buydate;
            CurrentRefund.Reason = reason;
            CurrentRefund.ProductId = productid;
            CurrentRefund.IsPurchased = true;
            CurrentRefund.IsSuccessRefund = false;
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(CurrentRefund);
        }

        public static async Task<RefundViewModel> CreateRefundDefect(DateOnly buydate, int productid)
        {
            CurrentRefund.BuyDate = buydate;
            CurrentRefund.Reason = "Брак";
            CurrentRefund.ProductId = productid;
            CurrentRefund.IsPurchased = true;
            CurrentRefund.IsSuccessRefund = false;
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(CurrentRefund);
        }

        public static async Task<List<RefundViewModel>> GetRefundedAllProduct() => await CashBoxDataContext.Context.Refunds
                                                                                .Select(s => new RefundViewModel(s))
                                                                                .ToListAsync();

        public static async Task<List<RefundViewModel>> GetRefundedDefect() => await CashBoxDataContext.Context.Refunds
                                                                            .Select(s => new RefundViewModel(s))
                                                                            .Where(x => x.IsPurchased == false && x.IsSuccessRefund == false)
                                                                            .ToListAsync();

        public static async Task<List<RefundViewModel>> GetRefundedReason() => await CashBoxDataContext.Context.Refunds
                                                                            .Select(s => new RefundViewModel(s))
                                                                            .Where(x => x.IsPurchased == true && x.IsSuccessRefund == false)
                                                                            .ToListAsync();
    }
}
