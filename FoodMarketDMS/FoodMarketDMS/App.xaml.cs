using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Modules.Offer;
using FoodMarketDMS.Modules.Service;
using FoodMarketDMS.Modules.User;
using FoodMarketDMS.Services;
using FoodMarketDMS.Services.Interfaces;
using FoodMarketDMS.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Reflection;
using System.Windows;

namespace FoodMarketDMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IExcelService, ExcelService>();
            containerRegistry.RegisterSingleton<IStateWrapperService, StateWrapperService>();

            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<UserModule>();
            moduleCatalog.AddModule<OfferModule>();
            moduleCatalog.AddModule<ServiceModule>();
        }

        /// <summary>
        /// Change naming convention in ViewModelLocator Autowire
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName}VM, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            });
        }

        #region Custom Exception Handler

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("In UnhandledException" + Environment.NewLine + ((Exception)e.ExceptionObject).Message);

            throw (Exception)e.ExceptionObject;

        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("In DispatcherUnhandledException" + Environment.NewLine + e.Exception.Message);

            throw e.Exception;
        }

        #endregion Custom Exception Handler
    }
}
