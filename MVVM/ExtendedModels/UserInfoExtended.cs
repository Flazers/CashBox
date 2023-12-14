using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class UserInfo
    {
        private UserInfo() { }

        // TODO: надо поменять на вьюмодел
        public static async Task<UserInfoViewModel?> CreateNewUserInfo(int userId, string name, string surname, string patronymic, string location, string phone, int roleId, bool isActive)
        {
            try
            {
                UserInfo userInfo = new UserInfo()
                {
                    UserId = userId,
                    Name = name,
                    Surname = surname,
                    Patronymic = patronymic,
                    Location = location,
                    Phone = phone,
                    RoleId = roleId,
                    IsActive = isActive
                };
                await CashBoxDataContext.Context.AddAsync(userInfo);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new UserInfoViewModel(userInfo);
            }
            catch (Exception) { return null; }
        }
    }
}
