using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;

namespace Cashbox.MVVM.Models
{
    public partial class UserInfo
    {
        private UserInfo() { }

        public static async Task<UserInfoViewModel?> CreateNewUserInfo(int userId, string name, string surname, string patronymic, string location, string phone, int roleId)
        {
            try
            {
                UserInfo userInfo = new()
                {
                    UserId = userId,
                    Name = name,
                    Surname = surname,
                    Patronymic = patronymic,
                    Location = location,
                    Phone = phone,
                    RoleId = roleId,
                    IsActive = true,
                    Salary = 0
                };
                await CashBoxDataContext.Context.AddAsync(userInfo);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new UserInfoViewModel(userInfo);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }

        public static async Task<UserInfoViewModel> DeactivateUser(int userId)
        {
            try
            {
                UserInfo user = CashBoxDataContext.Context.UserInfos.FirstOrDefault(x => x.UserId == userId);
                user.IsActive = false;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(user);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }

        public static async Task<UserInfoViewModel> ActivateUser(int userId)
        {
            try
            {
                UserInfo user = CashBoxDataContext.Context.UserInfos.FirstOrDefault(x => x.UserId == userId);
                user.IsActive = true;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(user);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }
    }
}
