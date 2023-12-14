using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class RoleViewModel
    {
        private readonly Role _role;

        public int Id => _role.Id;

        public string Role1 => _role.Role1;

        public RoleViewModel(Role role)
        {
            _role = role;
        }

        public static async Task<List<RoleViewModel>> GetRoles() => await Role.GetRoles();

    }
}
