using Cashbox.Core;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class HomeViewModel : ViewModelBase
    {

        #region Props

        private readonly UserViewModel? _user;
        public UserViewModel? User { 
            get => _user;
        }

        #endregion

        public HomeViewModel()
        {
            _user = Models.User.CurrentUser;
        }
    }
}
