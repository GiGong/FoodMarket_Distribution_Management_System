using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Events;
using FoodMarketDMS.Core.Extensions;
using FoodMarketDMS.Modules.User.Views;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
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
        private readonly IDialogService _dialogService;
        private readonly IStateWrapperService _stateWrapperService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        private DelegateCommand _loadUserListCommand;
        private DelegateCommand _addUserCommand;
        private DelegateCommand<UserClass> _editUserCommand;

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

        public DelegateCommand AddUserCommand =>
            _addUserCommand ??= new DelegateCommand(ExecuteAddUserCommand);

        public DelegateCommand<UserClass> EditUserCommand =>
            _editUserCommand ??= new DelegateCommand<UserClass>(ExecuteEditUserCommand);

        public UserListViewVM(IEventAggregator eventAggregator, IDialogService dialogService,
            IStateWrapperService stateWrapperService, IUserModuleCommands userModuleCommands, IFileService fileService, IExcelService excelService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _stateWrapperService = stateWrapperService;
            _fileService = fileService;
            _excelService = excelService;

            userModuleCommands.LoadUserListCommand.RegisterCommand(LoadUserListCommand);
            userModuleCommands.AddUserCommand.RegisterCommand(AddUserCommand);

            UserList = new ObservableCollection<UserClass>(_stateWrapperService.Users);
        }

        private void ExecuteLoadUserListCommand()
        {
            if (MessageBox.Show("이용자 목록을 초기화하면\n제공 목록이 초기화됩니다.\n계속 하시겠습니까?",
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
                users.Add(new UserClass(item[0], item[1], item[2]));
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunAsync((s, e) =>
            {
                UserList = new ObservableCollection<UserClass>(users);
                _stateWrapperService.Users = users;
                _eventAggregator.GetEvent<UserListChanged>().Publish();
            });
        }

        private void ExecuteAddUserCommand()
        {
            _dialogService.ShowDialog(nameof(UserEditView), null, 
                result => 
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    var resultUser = result.Parameters.GetValue<UserClass>(UserParameters.RESULT_USER);
                    UserList.Add(resultUser);
                    _stateWrapperService.Users.Add(resultUser);
                });
        }

        private void ExecuteEditUserCommand(UserClass user)
        {
            int index = UserList.IndexOf(user);
            _dialogService.ShowDialog(nameof(UserEditView), new DialogParameters { { UserParameters.CURRENT_USER, user} }, 
                result => 
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    var resultUser = result.Parameters.GetValue<UserClass>(UserParameters.RESULT_USER);
                    UserList[index] = resultUser;
                    _stateWrapperService.Users[index] = resultUser;
                });
        }


    }
}