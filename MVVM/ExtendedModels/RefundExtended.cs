using Cashbox.Core;
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
            try
            {
                var emptProducts = CashBoxDataContext.Context.Refunds.Where(x => x.ProductId == null).ToList();
                if (emptProducts.Count > 0)
                    CashBoxDataContext.Context.Refunds.RemoveRange(emptProducts);
                Refund refund = new()
                {
                    IsPurchased = false,
                    IsSuccessRefund = false,
                    DailyReportId = DailyReportViewModel.GetCurrentShift().Id,
                };
                CashBoxDataContext.Context.Refunds.Add(refund);
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentRefund = refund;
                return new(refund);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
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
            try
            {
                CurrentRefund.BuyDate = buydate;
                CurrentRefund.Reason = reason;
                CurrentRefund.ProductId = productid;
                CurrentRefund.IsPurchased = true;
                CurrentRefund.IsSuccessRefund = false;
                await CashBoxDataContext.Context.SaveChangesAsync();
                await CreateRefund();
                await CreateRefundDefect(productid, reason);
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }

        }

        public static async Task<bool> CreateRefundDefect(int productid, string reason)
        {
            try
            {
                CurrentRefund.Reason = reason;
                CurrentRefund.ProductId = productid;
                CurrentRefund.BuyDate = null;
                CurrentRefund.IsPurchased = false;
                CurrentRefund.IsSuccessRefund = false;
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentRefund = null;
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> CreateDraw(int productid, DateOnly datedraw)
        {
            try
            {
                CurrentRefund.Reason = "Розыгрыш";
                CurrentRefund.BuyDate = datedraw;
                CurrentRefund.ProductId = productid;
                CurrentRefund.IsPurchased = false;
                CurrentRefund.IsSuccessRefund = false;
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentRefund = null;
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> SuccessRefund()
        {
            try
            {
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }

        }

        public static async Task<List<RefundViewModel>> GetRefundedAllProduct() => await CashBoxDataContext.Context.Refunds
                                                                                .Select(s => new RefundViewModel(s))
                                                                                .ToListAsync();

        public static async Task<List<RefundViewModel>> GetRefundedDailyProduct(int DRid) => await CashBoxDataContext.Context.Refunds
                                                                        .Where(x => x.DailyReportId == DRid && x.IsPurchased == true)
                                                                        .Select(s => new RefundViewModel(s))
                                                                        .ToListAsync();

        public static async Task<List<RefundViewModel>> GetRefundedDefect() => await CashBoxDataContext.Context.Refunds
                                                                            .Where(x => x.IsPurchased == false)
                                                                            .Select(s => new RefundViewModel(s))
                                                                            .ToListAsync();

        public static async Task<List<RefundViewModel>> GetRefundedReason() => await CashBoxDataContext.Context.Refunds
                                                                            .Where(x => x.IsPurchased == true && x.BuyDate == null)
                                                                            .Select(s => new RefundViewModel(s))
                                                                            .ToListAsync();

        public static async Task<List<RefundViewModel>> GetDraw() => await CashBoxDataContext.Context.Refunds
                                                                    .Where(x => x.IsPurchased == false && x.BuyDate != null)
                                                                    .Select(s => new RefundViewModel(s))
                                                                    .ToListAsync();
    }
}
