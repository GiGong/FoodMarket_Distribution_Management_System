using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Mvvm;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserListViewVM : NavigationViewModelBase
    {
        private bool _isEnableGrid;
        private ObservableCollection<UserClass> _userList;

        private DelegateCommand _loadUserListCommand;

        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        public bool IsEnableGrid
        {
            get { return _isEnableGrid; }
            set { SetProperty(ref _isEnableGrid, value); }
        }

        public ObservableCollection<UserClass> UserList
        {
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }

        public DelegateCommand LoadUserListCommand =>
            _loadUserListCommand ??= new DelegateCommand(ExecuteLoadUserListCommand);


        public UserListViewVM(IApplicationCommands applicationCommands, IFileService fileService, IExcelService excelService)
        {
            applicationCommands.LoadUserListCommand.RegisterCommand(LoadUserListCommand);
            _fileService = fileService;
            _excelService = excelService;

        }

        private void ExecuteLoadUserListCommand()
        {
            string excelPath = _fileService.OpenFilePath(IExcelService.EXCEL_FILE_EXT, (string)Application.Current.Resources["Program_Name"]);
            var excelData = _excelService.GetExcelData(excelPath, typeof(UserClass).GetProperties().Count());
            if (excelData == null)
            {
                return;
            }

            List<UserClass> users = new List<UserClass>(excelData.GetLength(0));
            foreach (var item in excelData)
            {
                users.Add(new UserClass(item[0], item[1], item[2], item[3]));
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (s, e) =>
            {
                IsEnableGrid = false;
                UserList = new ObservableCollection<UserClass>(users);
                IsEnableGrid = true;
            }; ;

            backgroundWorker.RunWorkerAsync();
        }

    }
}