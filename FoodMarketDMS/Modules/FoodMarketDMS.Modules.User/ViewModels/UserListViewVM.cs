using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Events;
using FoodMarketDMS.Core.Extensions;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserListViewVM : NavigationViewModelBase
    {
        private bool _isEnableGrid;
        private ObservableCollection<UserClass> _userList;

        private readonly IEventAggregator _eventAggregator;
        private readonly IStateWrapperService _stateWrapperService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        private DelegateCommand _loadUserListCommand;

        public bool IsEnableGrid
        {
            get { return _isEnableGrid; }
            set { SetProperty(ref _isEnableGrid, value); }
        }

        public ObservableCollection<UserClass> UserList
        {
            get { return _userList; }
            set
            {
                SetProperty(ref _userList, value);
                _eventAggregator.GetEvent<UserCountChanged>().Publish(UserList.Count);
            }
        }


        public DelegateCommand LoadUserListCommand =>
            _loadUserListCommand ??= new DelegateCommand(ExecuteLoadUserListCommand);


        public UserListViewVM(IEventAggregator eventAggregator,
            IStateWrapperService stateWrapperService, IApplicationCommands applicationCommands, IFileService fileService, IExcelService excelService)
        {
            _eventAggregator = eventAggregator;
            _stateWrapperService = stateWrapperService;
            _fileService = fileService;
            _excelService = excelService;

            applicationCommands.LoadUserListCommand.RegisterCommand(LoadUserListCommand);

            UserList = new ObservableCollection<UserClass>(_stateWrapperService.Users);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        private void ExecuteLoadUserListCommand()
        {
            if (MessageBox.Show("이용자 목록을 새로 불러오면\n제공 목록이 초기화됩니다.\n계속 하시겠습니까?",
                (string)Application.Current.Resources["Program_Name"], MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No)
                == MessageBoxResult.No)
            {
                return;
            }

            string excelPath = _fileService.OpenFilePath(IExcelService.EXCEL_FILE_EXT, (string)Application.Current.Resources["Program_Name"]);
            var excelData = _excelService.GetExcelData(excelPath, typeof(UserClass).GetProperties().Count());
            if (excelData is null)
            {// fail to load excel data for any reason
                return;
            }

            List<UserClass> users = new List<UserClass>(excelData.GetLength(0));
            foreach (var item in excelData)
            {
                users.Add(new UserClass(item[0], item[1], item[2], item[3]));
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunAsync((s, e) =>
            {
                UserList = new ObservableCollection<UserClass>(users);
                _stateWrapperService.Users = users;
                _eventAggregator.GetEvent<UserListChanged>().Publish();
            });
        }
    }
}