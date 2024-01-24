using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class UserInfoViewModel : ViewModelBase
    {
        private readonly UserInfo _userInfo;
        public UserInfoViewModel(UserInfo userInfo)
        {
            _userInfo = userInfo;
            Role = new RoleViewModel(userInfo.Role);
            SetFullName();
        }

        public void SetFullName() { 
            FullName = $"{_userInfo.Surname} {_userInfo.Name} {_userInfo.Patronymic}";
            ShortName = $"{_userInfo.Surname} {_userInfo.Name[0]}. {_userInfo.Patronymic[0]}.";
        }

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            private set => Set(ref _fullName, value);
        }

        private string _shortName = string.Empty;
        public string ShortName
        {
            get => _shortName;
            private set => Set(ref _shortName, value);
        }

        public int UserId => _userInfo.UserId;

        public string Name
        {
            get => _userInfo.Name;
            set
            {
                _userInfo.Name = value;
                SetFullName();
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _userInfo.Surname;
            set
            {
                _userInfo.Surname = value;
                SetFullName();
                OnPropertyChanged();
            }
        }

        public string Patronymic
        {
            get => _userInfo.Patronymic;
            set
            {
                _userInfo.Patronymic = value;
                SetFullName();
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => _userInfo.Location;
            set
            {
                _userInfo.Location = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _userInfo.Phone;
            set
            {
                _userInfo.Phone = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _userInfo.IsActive;
            set
            {
                _userInfo.IsActive = value;
                OnPropertyChanged();
            }
        }

        public RoleViewModel Role { get; }
    }
}
