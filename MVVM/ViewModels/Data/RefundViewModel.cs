using Cashbox.Core;
using Cashbox.MVVM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class RefundViewModel(Refund refund) : ViewModelBase
    {
        private readonly Refund _refund = refund;

        public static Refund? CurrentRefund => Refund.CurrentRefund;
        public static async Task<RefundViewModel> CreateRefund() => await Refund.CreateRefund();
        public static async Task<RefundViewModel> RemoveCurrentRefund() => await Refund.RemoveCurrentRefund();
        public static async Task<RefundViewModel> CreateRefundReason(string reason, DateOnly buydate, int productid) => await Refund.CreateRefundReason(reason, buydate, productid);
        public static async Task<RefundViewModel> CreateRefundDefect(DateOnly buydate, int productid) => await Refund.CreateRefundDefect(buydate, productid);
        public static async Task<List<RefundViewModel>> GetRefundedAllProduct() => await Refund.GetRefundedAllProduct();
        public static async Task<List<RefundViewModel>> GetRefundedDefect() => await Refund.GetRefundedDefect();
        public static async Task<List<RefundViewModel>> GetRefundedReason() => await Refund.GetRefundedReason();

        public int Id => _refund.Id;

        public int? ProductId 
        {
            get => _refund.ProductId;
            set 
            {
                _refund.ProductId = value;
                OnPropertyChanged();
            }
        }

        public string? Reason
        {
            get => _refund.Reason;
            set
            {
                _refund.Reason = value;
                OnPropertyChanged();
            }
        }

        public DateOnly? BuyDate
        {
            get => _refund.BuyDate;
            set
            {
                _refund.BuyDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsPurchased
        {
            get => _refund.IsPurchased;
            set
            {
                _refund.IsPurchased = value;
                OnPropertyChanged();
            }
        }

        public bool IsSuccessRefund
        {
            get => _refund.IsSuccessRefund;
            set
            {
                _refund.IsSuccessRefund = value;
                OnPropertyChanged();
            }
        }

        public virtual Product? Product { get; set; } = null!;
    }
}
