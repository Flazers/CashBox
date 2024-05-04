using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class EmployeesViewModel : ViewModelBase
    {

        #region Props

        #region UserData

        private string _pincode = string.Empty;
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

        private Visibility _visibilityIsAdmin = Visibility.Collapsed;
        public Visibility VisibilityIsAdmin
        {
            get => _visibilityIsAdmin;
            set => Set(ref _visibilityIsAdmin, value);
        }

        private Visibility _visibilityIsUser = Visibility.Collapsed;
        public Visibility VisibilityIsUser
        {
            get => _visibilityIsUser;
            set => Set(ref _visibilityIsUser, value);
        }

        private Visibility _visibilityLog = Visibility.Collapsed;
        public Visibility VisibilityLog
        {
            get => _visibilityLog;
            set => Set(ref _visibilityLog, value);
        }

        private Visibility _visibilityMain = Visibility.Visible;
        public Visibility VisibilityMain
        {
            get => _visibilityMain;
            set => Set(ref _visibilityMain, value);
        }

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

        private ObservableCollection<UserViewModel> _collectionUsers = [];
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
                    if (value.UserInfo.RoleId == 1)
                    {
                        VisibilityIsAdmin = Visibility.Visible;
                        VisibilityIsUser = Visibility.Collapsed;
                    }
                    else
                    {
                        VisibilityIsAdmin = Visibility.Collapsed;
                        VisibilityIsUser = Visibility.Visible;
                    }

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

        private string _searchStr = string.Empty;
        public string SearchStr
        {
            get => _searchStr;
            set => Set(ref _searchStr, value);
        }

        private string _givenSalary = string.Empty;
        public string GivenSalary
        {
            get => _givenSalary;
            set => Set(ref _givenSalary, value);
        }

        #endregion

        #region Commands

        public RelayCommand SearchDataCommand { get; set; }
        private bool CanSearchDataCommandExecute(object p) => true;
        private void OnSearchDataCommandExecuted(object p) => Update();

        public RelayCommand GiveSalaryCommand { get; set; }
        private bool CanGiveSalaryCommandExecute(object p) => true;
        private async void OnGiveSalaryCommandExecuted(object p)
        {
            if (SelectedUser == null) return;
            if (string.IsNullOrEmpty(GivenSalary))
            {
                AppCommand.WarningMessage("Введите сколько выдать.");
                return;
            }
            if (int.Parse(GivenSalary.Trim()) > SelectedUser.UserInfo.Salary)
            {
                AppCommand.WarningMessage("Нельзя выдать зарплату больше чем начислено.");
                return;
            }
            await AdminMoneyLogViewModel.CreateTransitSalary($"Администратор {UserViewModel.GetCurrentUser().UserInfo.ShortName} выдал зарплату сотруднику {SelectedUser.UserInfo.FullName} в размере {GivenSalary} ₽", double.Parse(GivenSalary), SelectedUser.Id);
            await UserViewModel.TakeSalary(SelectedUser, double.Parse(GivenSalary));
            AppCommand.InfoMessage("Зарплата выдана");
            Update();
        }

        public RelayCommand SeeLogCommand { get; set; }
        private bool CanSeeLogCommandExecute(object p) => true;
        private void OnSeeLogCommandExecuted(object p)
        {
            if (VisibilityLog == Visibility.Collapsed)
            {
                VisibilityLog = Visibility.Visible;
                VisibilityMain = Visibility.Collapsed;
            }
            else
            {
                VisibilityLog = Visibility.Collapsed;
                VisibilityMain = Visibility.Visible;
            }

        }

        public RelayCommand SeeUserInfoCommand { get; set; }
        private bool CanSeeUserInfoCommandExecute(object p) => true;
        private void OnSeeUserInfoCommandExecuted(object p)
        {
            UserViewModel? user = CollectionUsers.FirstOrDefault(x => x.Id == int.Parse(p.ToString()!));
            if (user == null) return;
            SelectedUser = user;
            GivenSalary = SelectedUser.UserInfo.Salary.ToString();
        }

        public RelayCommand CreateUserCommand { get; set; }
        private bool CanCreateUserCommandExecute(object p) => true;
        private async void OnCreateUserCommandExecuted(object p)
        {
            if (Pincode.ToString().Length != 6 || Name == null || Surname == null || Patronymic == null || Location == null || Phone == null || Role == null)
            {
                AppCommand.WarningMessage("Заполните все поля");
                return;
            }
            UserViewModel user = await UserViewModel.CreateUser(PincodeInt, Name, Surname, Patronymic, Location, Phone, Role);
            if (user == null)
            {
                AppCommand.ErrorMessage("Не удалось создать пользователя с данным пинкодом");
                return;
            }
            CollectionUsers = new(await UserViewModel.GetListUsers());
            AppCommand.InfoMessage("Пользователь создан");
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
            if (AppCommand.QuestionMessage($"Вы уверены, что хотите уволить сотрудника {SelectedUser.UserInfo.FullName}?") == MessageBoxResult.Yes)
            {
                await UserInfoViewModel.DeactivateUser(SelectedUser.Id);
                await AdminMoneyLogViewModel.CreateTransitSalary($"Администратор {UserViewModel.GetCurrentUser().UserInfo.ShortName} уволил сотрудника {SelectedUser.UserInfo.FullName}", 0, SelectedUser.Id);
                CollectionUsers.Remove(SelectedUser);
                SelectedUser = null;
                AppCommand.InfoMessage("Пользователь уволен");
            }
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
            if (SelectedUser == null)
                return;
            if (await UserViewModel.EditUser(SelectedUser))
            {
                AppCommand.ErrorMessage("Ошибка при редактировании пользователя");
                return;
            }
            await AdminMoneyLogViewModel.CreateTransitSalary($"Администратор {UserViewModel.GetCurrentUser().UserInfo.ShortName} отредактировал сотрудника под id {SelectedUser.Id} ({SelectedUser.UserInfo.FullName})", 0, SelectedUser.Id);
            AppCommand.InfoMessage("Пользователь отредактирован");
            VisibilityEditUserPanel = Visibility.Collapsed;
            VisibilityUserInfoPanel = Visibility.Visible;
        }

        private async void Update()
        {
            List<UserViewModel> data = await UserViewModel.GetListUsers();
            if (string.IsNullOrEmpty(SearchStr))
                CollectionUsers = new(data.OrderBy(x => x.UserInfo.RoleId));
            else
                CollectionUsers = new(data.Where(x => x.UserInfo.FullName.Trim().Contains(SearchStr.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).OrderBy(x => x.UserInfo.RoleId));
        }

        #endregion

        public EmployeesViewModel()
        {
            Update();
            SeeLogCommand = new RelayCommand(OnSeeLogCommandExecuted, CanSeeLogCommandExecute);
            GiveSalaryCommand = new RelayCommand(OnGiveSalaryCommandExecuted, CanGiveSalaryCommandExecute);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
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
