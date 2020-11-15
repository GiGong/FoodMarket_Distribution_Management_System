using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Extensions;
using FoodMarketDMS.Core.Mvvm;
using FoodMarketDMS.Modules.Offer.Views;
using FoodMarketDMS.Modules.Service.Views;
using FoodMarketDMS.Modules.User.Views;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace FoodMarketDMS.ViewModels
{
    public class MainWindowVM : BindableBase, IWindowLoadedLoader, IClosingWindow
    {// 검색기능, 이용자 추가, 이용자 포맷 맞추기, 이용자 정보 변경
        
        private string _title;
        private string _viewTitle;

        private readonly IRegionManager _regionManager;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IStateWrapperService _stateWrapperService;
        private IApplicationCommands _applicationCommands;

        private DelegateCommand _saveStateCommand;
        private DelegateCommand _navigateUserListCommand;
        private DelegateCommand _navigateServiceListCommand;
        private DelegateCommand _navigateOfferListCommand;
        private DelegateCommand _exportToExcelCommand;

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

        public DelegateCommand ExportToExcelCommand =>
            _exportToExcelCommand ??= new DelegateCommand(ExecuteExportToExcelCommand);


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
            AutoResetEvent _doneEvent = new AutoResetEvent(false);
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunAsync(
                (s, e) =>
                {
                    _stateWrapperService.LoadState();
                    _doneEvent.Set();
                });

            _doneEvent.WaitOne();
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

        private void ExecuteExportToExcelCommand()
        {
            string path = _fileService.SaveFilePath(IExcelService.EXCEL_FILE_EXT, Title);
            if (!string.IsNullOrWhiteSpace(path))
            {
                _stateWrapperService.ExportToExcel(path, _excelService);
            }
        }

    }
}
