using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ComingProductViewModel(ComingProduct comingProduct) : ViewModelBase
    {
        private readonly ComingProduct _comingProduct = comingProduct;

        public static async Task<bool> NewComing(double BuyCost) => await ComingProduct.NewComing(BuyCost);
        public static async Task<List<ComingProductViewModel>> GetComing() => await ComingProduct.GetComing();

        public int Id => _comingProduct.Id;
        public int UserId => _comingProduct.UserId;
        public DateTime CommingDatetime => _comingProduct.CommingDatetime;
        public string? DataString => CommingDatetime.ToString("dd/MM/yyyy HH:mm:ss");
        public double BuyCost => _comingProduct.BuyCost;
        public virtual User User { get; set; } = null!;
    }
}
