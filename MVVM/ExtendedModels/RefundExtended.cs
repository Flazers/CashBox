using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

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

        public static async Task<bool> RemoveCurrentRefund()
        {
            try
            {
                CashBoxDataContext.Context.Refunds.Remove(CurrentRefund!);
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentRefund = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> CreateRefundReason(string reason, DateOnly buydate, int productid)
        {
            CurrentRefund.BuyDate = buydate;
            CurrentRefund.Reason = reason;
            CurrentRefund.ProductId = productid;
            CurrentRefund.DailyReportId = DailyReportViewModel.CurrentShift.Id;
            CurrentRefund.IsPurchased = true;
            CurrentRefund.IsSuccessRefund = false;
            await CashBoxDataContext.Context.SaveChangesAsync();
            CurrentRefund = null;
            return true;
        }

        public static async Task<bool> CreateRefundDefect(int productid)
        {
            CurrentRefund.Reason = "Брак";
            CurrentRefund.ProductId = productid;
            CurrentRefund.DailyReportId = DailyReportViewModel.CurrentShift.Id;
            CurrentRefund.IsPurchased = false;
            CurrentRefund.IsSuccessRefund = false;
            await CashBoxDataContext.Context.SaveChangesAsync();
            CurrentRefund = null;
            return true;
        }

        public static async Task<bool> CreateDraw(int productid)
        {
            CurrentRefund.Reason = "Розыгрыш";
            CurrentRefund.ProductId = productid;
            CurrentRefund.DailyReportId = DailyReportViewModel.CurrentShift.Id;
            CurrentRefund.IsPurchased = false;
            CurrentRefund.IsSuccessRefund = false;
            await CashBoxDataContext.Context.SaveChangesAsync();
            CurrentRefund = null;
            return true;
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
