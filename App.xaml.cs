using Cashbox.Core;
using Cashbox.MVVM.ViewModels;
using Cashbox.MVVM.ViewModels.Admin;
using Cashbox.MVVM.ViewModels.Employee;
using Cashbox.Service;
using Cashbox.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}