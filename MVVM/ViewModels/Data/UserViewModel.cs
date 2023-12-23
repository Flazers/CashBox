﻿using Cashbox.Core;
using Cashbox.MVVM.Models;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class UserViewModel : ViewModelBase
    {
        private readonly User _user;
        public UserViewModel(User user)
        {
            _user = user;
            UserInfo = new UserInfoViewModel(user.UserInfo!);
        }

        public static UserViewModel? GetCurrentUser() => User.CurrentUser;

        public static void LogOut() => User.LogOut();
        

        public static async Task<UserViewModel?> GetUserByLogPass(string login, string password) => await User.GetUserByLogPass(login, password);
        

        public static async Task<UserViewModel?> GetUserByPin(int pincode) => await User.GetUserByPin(pincode);
        

        public static async Task<UserViewModel?> CreateUser(string login, string password, int pincode, bool TFA, string name, string surname, string patronymic, string location, string phone, RoleViewModel role, bool isActive) => await User.CreateUser(login, password, pincode, TFA, name, surname, patronymic, location, phone, role, isActive);
        

        public void SetUserInfo(UserInfoViewModel? userInfoViewModel)
        {
            if (userInfoViewModel == null) return;
            UserInfo = userInfoViewModel;
        }


        public UserInfoViewModel UserInfo { get; private set; }

        public int Id => _user.Id;

        public bool Tfa => _user.Tfa;

        public virtual ICollection<AuthHistory> AuthHistories => _user.AuthHistories;

        public virtual ICollection<DailyReport> DailyReports => _user.DailyReports;

        public virtual ICollection<Order> Orders => _user.Orders;
    }
}
