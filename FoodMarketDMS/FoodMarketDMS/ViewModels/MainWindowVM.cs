using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Mvvm;
using FoodMarketDMS.Modules.Offer.Views;
using FoodMarketDMS.Modules.Service.Views;
using FoodMarketDMS.Modules.User.Views;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace FoodMarketDMS.ViewModels
{
    public class MainWindowVM : BindableBase, IWindowLoadedLoader, IClosingWindow
    {
        private string _title;
        private string _viewTitle;

        private readonly IRegionManager _regionManager;
        private IApplicationCommands _applicationCommands;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IStateWrapperService _stateWrapperService;

        private DelegateCommand _saveStateCommand;
        private DelegateCommand _navigateUserListCommand;
        private DelegateCommand _navigateServiceListCommand;
        private DelegateCommand _navigateOfferListCommand;

        public Action Close { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string ViewTitle
        {
            get { return _viewTitle; }
            set { SetProperty(ref _viewTitle, value); }
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }


        public DelegateCommand SaveStateCommand =>
            _saveStateCommand ??= new DelegateCommand(ExecuteSaveStateCommand);

        public DelegateCommand NavigateUserListCommand =>
            _navigateUserListCommand ??= new DelegateCommand(ExecuteNavigateUserListCommand);

        public DelegateCommand NavigateServiceListCommand =>
            _navigateServiceListCommand ??= new DelegateCommand(ExecuteNavigateServiceListCommand);

        public DelegateCommand NavigateOfferListCommand =>
            _navigateOfferListCommand ??= new DelegateCommand(ExecuteNavigateOfferListCommand);


        public MainWindowVM(IRegionManager regionManager, IApplicationCommands applicationCommands,
                            IFileService fileService, IExcelService excelService, IStateWrapperService stateWrapperService)
        {
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _fileService = fileService;
            _excelService = excelService;
            _stateWrapperService = stateWrapperService;

            Title = (string)Application.Current.Resources["Program_Name"];
            ViewTitle = ViewNames.UserListView;
        }

        public void WindowLoaded()
        {
            ExecuteNavigateServiceListCommand();
            ExecuteNavigateOfferListCommand();
            ExecuteNavigateUserListCommand();
        }

        private void ExecuteSaveStateCommand()
        {
            _stateWrapperService.SaveState();
        }

        public void WindowClosing(object sender, CancelEventArgs e)
        {
            SaveStateCommand.Execute();
        }

        private void ExecuteNavigateUserListCommand()
        {
            ViewTitle = ViewNames.UserListView;
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(UserListView));
            _regionManager.RequestNavigate(RegionNames.MenuRegion, nameof(UserMenuView));
        }

        private void ExecuteNavigateServiceListCommand()
        {
            ViewTitle = ViewNames.ServiceListView;
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(ServiceListView));
            _regionManager.RequestNavigate(RegionNames.MenuRegion, nameof(ServiceMenuView));
        }

        private void ExecuteNavigateOfferListCommand()
        {
            ViewTitle = ViewNames.OfferListView;
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(OfferListView));
            _regionManager.RequestNavigate(RegionNames.MenuRegion, nameof(OfferMenuView));
        }

    }
}
