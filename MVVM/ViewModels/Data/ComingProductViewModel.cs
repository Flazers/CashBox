using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ComingProductViewModel(ComingProduct comingProduct) : ViewModelBase
    {
        private readonly ComingProduct _comingProduct = comingProduct;


        public int Id => _comingProduct.Id;
        public int UserId => _comingProduct.UserId;
        public DateTime CommingDatetime => _comingProduct.CommingDatetime;
        public virtual User User { get; set; } = null!;
    }
}
