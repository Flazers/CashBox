using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class EmployeesViewModel : ViewModelBase
    {

        #region Props
        private UserViewModel? _user;
        public UserViewModel? User { get => _user = Models.User.CurrentUser; }
        #endregion
    }
}
