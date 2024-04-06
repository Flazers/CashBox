using Cashbox.Core;
using Cashbox.MVVM.Models;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{

    public class UserViewModel(User user) : ViewModelBase
    {
        private readonly User _user = user;
        public UserInfoViewModel UserInfo { get; private set; } = new UserInfoViewModel(user.UserInfo!);

        public static UserViewModel? GetCurrentUser() => User.CurrentUser;
        public static void LogOut() => User.LogOut();
        public static async Task<UserViewModel?> GetUserByLogPass(string login, string password) => await User.GetUserByLogPass(login, password);
        public static async Task<UserViewModel?> GetUserByPin(int pincode) => await User.GetUserByPin(pincode);
        public static async Task<UserViewModel?> CreateUser(string login, string password, int pincode, string name, string surname, string patronymic, string location, string phone, RoleViewModel role) => await User.CreateUser(login, password, pincode, name, surname, patronymic, location, phone, role);
        public static async Task<List<UserViewModel>> GetListUsers() => await User.GetListUsers();

        public void SetUserInfo(UserInfoViewModel? userInfoViewModel)
        {
            if (userInfoViewModel == null) return;
            UserInfo = userInfoViewModel;
        }

        public int Id => _user.Id;

        public int Pin
        {
            get => _user.Pin;
            set { 
                _user.Pin = value; 
                OnPropertyChanged();
            }
        }

        public virtual ICollection<AuthHistory> AuthHistories => _user.AuthHistories;

        public virtual ICollection<DailyReport> DailyReports => _user.DailyReports;

        public virtual ICollection<Order> Orders => _user.Orders;
    }
}
