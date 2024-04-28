using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class RoleViewModel(Role role) : ViewModelBase
    {
        private readonly Role _role = role;

        public static async Task<List<RoleViewModel>> GetRoles() => await Role.GetRoles();

        public int Id => _role.Id;

        public string Role1
        {
            get => _role.Role1;
            set
            {
                _role.Role1 = value;
                OnPropertyChanged();
            }
        }
    }
}
