using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class EmployeesViewModel : ViewModelBase
    {

        #region Props

        #region UserData

        private string _pincode;
        public string Pincode
        {
            get => _pincode;
            set => Set(ref _pincode, value);
        }

        public int PincodeInt => int.Parse(Pincode);
        

        private string? _name;
        public string? Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string? _surname;
        public string? Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        private string? _patronymic;
        public string? Patronymic
        {
            get => _patronymic;
            set => Set(ref _patronymic, value);
        }

        private string? _location;
        public string? Location
        {
            get => _location;
            set => Set(ref _location, value);
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        private RoleViewModel? _role;
        public RoleViewModel? Role
        {
            get => _role;
            set => Set(ref _role, value);
        }

        private bool? _isActive;
        public bool? IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

        #endregion

        #region Visibility

        private Visibility _visibilityUserInfoPanel = Visibility.Visible;
        public Visibility VisibilityUserInfoPanel
        {
            get => _visibilityUserInfoPanel;
            set => Set(ref _visibilityUserInfoPanel, value);
        }

        private Visibility _visibilityCreateUserPanel = Visibility.Collapsed;
        public Visibility VisibilityCreateUserPanel
        {
            get => _visibilityCreateUserPanel;
            set => Set(ref _visibilityCreateUserPanel, value);
        }

        private Visibility _userSelectedPanel = Visibility.Collapsed;
        public Visibility UserSelectedPanel
        {
            get => _userSelectedPanel;
            set => Set(ref _userSelectedPanel, value);
        }

        private Visibility _notUserSelectedPanel = Visibility.Visible;
        public Visibility NotUserSelectedPanel
        {
            get => _notUserSelectedPanel;
            set => Set(ref _notUserSelectedPanel, value);
        }

        private Visibility _visibilityEditUserPanel = Visibility.Collapsed;
        public Visibility VisibilityEditUserPanel
        {
            get => _visibilityEditUserPanel;
            set => Set(ref _visibilityEditUserPanel, value);
        }

        #endregion

        private ObservableCollection<RoleViewModel> _collectionUserRoles = new(RoleViewModel.GetRoles().Result);
        public ObservableCollection<RoleViewModel> CollectionUserRoles
        {
            get => _collectionUserRoles;
            set => Set(ref _collectionUserRoles, value);
        }

        private ObservableCollection<UserViewModel> _collectionUsers = new(UserViewModel.GetListUsers().Result);
        public ObservableCollection<UserViewModel> CollectionUsers
        {
            get => _collectionUsers;
            set => Set(ref _collectionUsers, value);
        }

        private UserViewModel? _selectedUser;
        public UserViewModel? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotUserSelectedPanel = Visibility.Visible;
                UserSelectedPanel = Visibility.Collapsed;
                
                if (value != null)
                {
                    NotUserSelectedPanel = Visibility.Collapsed;
                    UserSelectedPanel = Visibility.Visible;
                }
                else
                {
                    NotUserSelectedPanel = Visibility.Visible;
                    UserSelectedPanel = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public RelayCommand SeeUserInfoCommand { get; set; }
        private bool CanSeeUserInfoCommandExecute(object p) => true;
        private void OnSeeUserInfoCommandExecuted(object p)
        {
            UserViewModel? user = CollectionUsers.FirstOrDefault(x => x.Id == int.Parse(p.ToString()!));
            if (user == null) return;
            SelectedUser = user;
        }

        public RelayCommand CreateUserCommand { get; set; }
        private bool CanCreateUserCommandExecute(object p) => true;
        private async void OnCreateUserCommandExecuted(object p)
        {
            if (Pincode.ToString().Length != 6 || Name == null || Surname == null || Patronymic == null || Location == null || Phone == null || Role == null) return;
            UserViewModel user = await UserViewModel.CreateUser(PincodeInt, Name, Surname, Patronymic, Location, Phone, Role);
            if (user == null)
            {
                MessageBox.Show("Не удалось создать пользователя с данным пинкодом", "Ошибка");
                return;
            }
            CollectionUsers = new(UserViewModel.GetListUsers().Result);
            MessageBox.Show("Пользователь создан", "Успех");
            SelectedUser = user;
            OnSeeUserInfoPanelVisibilityCommandExecuted(p);
        }

        public RelayCommand AddUserPanelVisibilityCommand { get; set; }
        private bool CanAddUserPanelVisibilityCommandExecute(object p) => true;
        private void OnAddUserPanelVisibilityCommandExecuted(object p)
        {
            VisibilityUserInfoPanel = Visibility.Collapsed;
            VisibilityCreateUserPanel = Visibility.Visible;
        }

        public RelayCommand SeeUserInfoPanelVisibilityCommand { get; set; }
        private bool CanSeeUserInfoPanelVisibilityCommandExecute(object p) => true;
        private void OnSeeUserInfoPanelVisibilityCommandExecuted(object p)
        {
            VisibilityUserInfoPanel = Visibility.Visible;
            VisibilityCreateUserPanel = Visibility.Collapsed;
        }

        public RelayCommand RemoveEmployeeCommand { get; set; }
        private bool CanRemoveEmployeeCommandExecute(object p) => true;
        private async void OnRemoveEmployeeCommandExecuted(object p) 
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите уволить сотрудника {SelectedUser.UserInfo.FullName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            await UserInfoViewModel.DeactivateUser(SelectedUser.Id);
            CollectionUsers.Remove(SelectedUser);
            MessageBox.Show("Успех");
        }
        public RelayCommand OpenPanelEmployeeEditCommand { get; set; }
        private bool CanOpenPanelEmployeeEditCommandExecute(object p) => true;
        private void OnOpenPanelEmployeeEditCommandExecuted(object p)
        {
            VisibilityEditUserPanel = Visibility.Visible;
            VisibilityUserInfoPanel = Visibility.Collapsed;
        }

        public RelayCommand ClosePanelEditUserCommand { get; set; }
        private bool CanClosePanelEditUserCommandExecute(object p) => true;
        private void OnClosePanelEditUserCommandExecuted(object p)
        {
            VisibilityEditUserPanel = Visibility.Collapsed;
            VisibilityUserInfoPanel = Visibility.Visible;
        }

        public RelayCommand EditUserCommand { get; set; }
        private bool CanEditUserCommandExecute(object p) => true;
        private async void OnEditUserCommandExecuted(object p)
        {
            UserViewModel userVM = await UserViewModel.EditUser(SelectedUser);
            if (userVM == null)
            {
                MessageBox.Show("Ошибка при редактировании пользователя");
                return;
            }
            MessageBox.Show("Пользователь отредактирован", "Успех");
            VisibilityEditUserPanel = Visibility.Collapsed;
            VisibilityUserInfoPanel = Visibility.Visible;
        }

        #endregion

        public EmployeesViewModel()
        {
            SeeUserInfoCommand = new RelayCommand(OnSeeUserInfoCommandExecuted, CanSeeUserInfoCommandExecute);
            AddUserPanelVisibilityCommand = new RelayCommand(OnAddUserPanelVisibilityCommandExecuted, CanAddUserPanelVisibilityCommandExecute);
            SeeUserInfoPanelVisibilityCommand = new RelayCommand(OnSeeUserInfoPanelVisibilityCommandExecuted, CanSeeUserInfoPanelVisibilityCommandExecute);
            CreateUserCommand = new RelayCommand(OnCreateUserCommandExecuted, CanCreateUserCommandExecute);
            OpenPanelEmployeeEditCommand = new RelayCommand(OnOpenPanelEmployeeEditCommandExecuted, CanOpenPanelEmployeeEditCommandExecute);
            RemoveEmployeeCommand = new RelayCommand(OnRemoveEmployeeCommandExecuted, CanRemoveEmployeeCommandExecute);
            ClosePanelEditUserCommand = new RelayCommand(OnClosePanelEditUserCommandExecuted, CanClosePanelEditUserCommandExecute);
            EditUserCommand = new RelayCommand(OnEditUserCommandExecuted, CanEditUserCommandExecute);
        }
    }
}
