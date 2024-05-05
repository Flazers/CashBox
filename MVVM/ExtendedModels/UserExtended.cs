using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class User
    {
        private User() { }

        private static UserViewModel? _currentUser;
        public static UserViewModel? CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                if (value != null)
                    if (_currentUser?.UserInfo.Role.Id != 2)
                        SetAuth();
            }
        }

        private static async void SetAuth() => await AuthHistory.NewAuthUser();

        public static async Task<UserViewModel?> GetUserByPin(int pincode)
        {
            User? user = await CashBoxDataContext.Context.Users.FirstOrDefaultAsync(x => x.Pin == pincode && x.UserInfo.IsActive == true);
            return user != null ? CurrentUser = new UserViewModel(user) : null;
        }

        public static void LogOut()
        {
            CurrentUser = null;
        }

        public static async Task<UserViewModel?> CreateUser(int pincode, string name, string surname, string patronymic, string location, string phone, RoleViewModel role)
        {
            try
            {
                if (CashBoxDataContext.Context.Users.FirstOrDefault(x => x.Pin == pincode) != null) { return null; }
                User user = new()
                {
                    Pin = pincode,
                };
                await CashBoxDataContext.Context.Users.AddAsync(user);
                await CashBoxDataContext.Context.SaveChangesAsync();
                UserInfoViewModel? userinfoVM = await UserInfo.CreateNewUserInfo(user.Id, name, surname, patronymic, location, phone, role.Id);
                UserViewModel userVM = new(user);
                userVM.SetUserInfo(userinfoVM);
                return userVM;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null;
            }
        }

        public static async Task<bool> EditUser(UserViewModel userVMdata)
        {
            try
            {
                User user = CashBoxDataContext.Context.Users.FirstOrDefault(x => x.Id == userVMdata.Id);
                if (user == null) return false;
                user.Pin = userVMdata.Pin;
                user.UserInfo.Name = userVMdata.UserInfo.Name;
                user.UserInfo.Surname = userVMdata.UserInfo.Surname;
                user.UserInfo.Patronymic = userVMdata.UserInfo.Patronymic;
                user.UserInfo.Phone = userVMdata.UserInfo.Phone;
                user.UserInfo.Location = userVMdata.UserInfo.Location;
                user.UserInfo.RoleId = userVMdata.UserInfo.RoleId;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> TakeSalary(UserViewModel userVMdata, double money)
        {
            try
            {
                User user = CashBoxDataContext.Context.Users.FirstOrDefault(x => x.Id == userVMdata.Id);
                if (user == null) return false;
                user.UserInfo.Salary -= money;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<List<UserViewModel>> GetListUsers() => await CashBoxDataContext.Context.Users.Where(x => x.UserInfo.IsActive == true).Select(s => new UserViewModel(s)).ToListAsync();
    }
}
