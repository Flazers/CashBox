using Cashbox.Service;
using Cashbox.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Cashbox.MVVM.ViewModels;
using Cashbox.Core;
using Cashbox.MVVM.Models;
using Microsoft.EntityFrameworkCore;
using Cashbox.MVVM.ViewModels.Data;
using System.Data;
using Cashbox.MVVM.ViewModels.Admin;
using Cashbox.MVVM.ViewModels.Employee;

namespace Cashbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoadingViewModel>();
            services.AddSingleton<AuthViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<AMainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<EmployeesViewModel>();
            services.AddSingleton<StockViewModel>();
            services.AddSingleton<MVVM.ViewModels.Admin.ShiftViewModel>();
            services.AddSingleton<EMainViewModel>();
            services.AddSingleton<CashRegisterViewModel>();
            services.AddSingleton<MVVM.ViewModels.Employee.ShiftViewModel>();
            services.AddSingleton<ISubNavigationService, SubNavigationService>();

            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => ViewModelType => (ViewModelBase)serviceProvider.GetRequiredService(ViewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}