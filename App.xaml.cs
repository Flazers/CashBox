﻿using Cashbox.Service;
using Cashbox.MVVM.Views.Pages;
using Cashbox.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using Cashbox.MVVM.ViewModels;
using Cashbox.MVVM.ViewModels.Base;
using System.IO.Packaging;
using System;

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
            services.AddSingleton<LoadAppViewModel>();
            services.AddSingleton<AuthViewModel>(provider => new AuthViewModel(provider.GetRequiredService<INavigationService>()));
            services.AddSingleton<LoadingViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

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