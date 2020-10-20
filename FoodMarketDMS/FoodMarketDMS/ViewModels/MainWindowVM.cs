using FoodMarketDMS.Modules.User.Views;
using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using FoodMarketDMS.Modules.Service.Views;
using FoodMarketDMS.Modules.Offer.Views;
using FoodMarketDMS.Core.Mvvm;

namespace FoodMarketDMS.ViewModels
{
    public class MainWindowVM : BindableBase, IWindowLoadedLoader
    {
        private string _title;
        private string _viewTitle;

        private readonly IRegionManager _regionManager;
        private IApplicationCommands _applicationCommands;

        private DelegateCommand _navigateUserListCommand;
        private DelegateCommand _navigateServiceListCommand;
        private DelegateCommand _navigateOfferListCommand;

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

        public DelegateCommand NavigateUserListCommand =>
            _navigateUserListCommand ??= new DelegateCommand(ExecuteNavigateUserListCommand);

        public DelegateCommand NavigateServiceListCommand =>
            _navigateServiceListCommand ??= new DelegateCommand(ExecuteNavigateServiceListCommand);

        public DelegateCommand NavigateOfferListCommand =>
            _navigateOfferListCommand ??= new DelegateCommand(ExecuteNavigateOfferListCommand);

        public MainWindowVM(IApplicationCommands applicationCommands, IRegionManager regionManager)
        {
            _applicationCommands = applicationCommands;
            _regionManager = regionManager;

            Title = (string)Application.Current.Resources["Program_Name"];
            ViewTitle = ViewNames.UserListView;
        }

        public void WindowLoaded()
        {
            ExecuteNavigateServiceListCommand();
            ExecuteNavigateOfferListCommand();
            ExecuteNavigateUserListCommand();
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
