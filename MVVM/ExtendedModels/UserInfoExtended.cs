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

        public static async Task<UserInfoViewModel?> CreateNewUserInfo(int userId, string name, string surname, string patronymic, string location, string phone, int roleId)
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
                    IsActive = true
                };
                await CashBoxDataContext.Context.AddAsync(userInfo);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new UserInfoViewModel(userInfo);
            }
            catch (Exception) { return null; }
        }
    }
}
