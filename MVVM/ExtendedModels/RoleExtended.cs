using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class Role
    {
        private Role() { }
        private static async Task<List<RoleViewModel>> CreateBaseRoles()
        {
            List<Role> roles = new List<Role>();
            try
            {
                Role SuperUser = new Role() { Id = 1, Role1 = "Админ+" };
                Role adminRole = new Role() { Id = 2, Role1 = "Админ" };
                Role EmployeeRole = new Role() { Id = 3, Role1 = "Сотрудник" };
                roles.Add(SuperUser);
                roles.Add(adminRole);
                roles.Add(EmployeeRole);
                CashBoxDataContext.Context.Roles.AddRange(roles);
                await CashBoxDataContext.Context.SaveChangesAsync();
            }
            catch (Exception) { return new List<RoleViewModel>(); }

            return roles.Select(s => new RoleViewModel(s)).ToList();
        }

        public static async Task<List<RoleViewModel>> GetRoles()
        {
            List<RoleViewModel> roles;

            if (CashBoxDataContext.Context.Roles.Count() == 0)
                roles = await CreateBaseRoles();
            else
                roles = await CashBoxDataContext.Context.Roles.Select(s => new RoleViewModel(s)).ToListAsync();
            
            return roles;
        }

    }
}
