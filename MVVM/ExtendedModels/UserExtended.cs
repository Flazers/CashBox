using BCrypt.Net;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class User
    {
        private User() { }

        private static UserViewModel? _currentUser;
        public static UserViewModel? CurrentUser
        {
            get => _currentUser;
            private set {
                _currentUser = value;
                if (value != null)
                    if (_currentUser?.UserInfo.Role.Id != 3)
                        SetAuth();
            }
        }

        private static async void SetAuth() => await AuthHistory.NewAuthUser();

        public static async Task<UserViewModel?> GetUserByLogPass(string login, string password)
        {
            User? user = await CashBoxDataContext.Context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;
            return CurrentUser = new UserViewModel(user);
        }

        public static async Task<UserViewModel?> GetUserByPin(int pincode)
        {
            User? user = await CashBoxDataContext.Context.Users.FirstOrDefaultAsync(x => x.Pin == pincode);
            return user != null ? CurrentUser = new UserViewModel(user) : null;
        }

        public static void LogOut()
        {
            CurrentUser = null;
        }

        public static async Task<UserViewModel?> CreateUser(string login, string password, int pincode, bool TFA, string name, string surname, string patronymic, string location, string phone, RoleViewModel role, bool isActive)
        {
            try
            {
                User user = new User
                {
                    Login = login,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    Pin = pincode,
                    Tfa = TFA,
                };
                await CashBoxDataContext.Context.Users.AddAsync(user);
                await CashBoxDataContext.Context.SaveChangesAsync();
                UserInfoViewModel? userinfoVM = await UserInfo.CreateNewUserInfo(user.Id, name, surname, patronymic, location, phone, role.Id, isActive);
                UserViewModel userVM = new UserViewModel(user);
                userVM.SetUserInfo(userinfoVM);
                return userVM;
            }
            catch (Exception) { return null; }
        }
    }
}
