using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        private OrderViewModel? _selectedOrder;
        public OrderViewModel? SelectedOrder 
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }
        #endregion
    }
}
