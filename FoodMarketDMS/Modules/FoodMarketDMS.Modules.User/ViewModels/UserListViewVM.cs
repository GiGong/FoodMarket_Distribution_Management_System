using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Mvvm;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserListViewVM : NavigationViewModelBase
    {
        private ObservableCollection<Person> _userList;

        private DelegateCommand _loadUserListCommand;

        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        public ObservableCollection<Person> UserList
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
            var excelData = _excelService.GetExcelData(excelPath, typeof(Person).GetProperties().Count());
            if (excelData == null)
            {
                return;
            }

            List<Person> users = new List<Person>(excelData.GetLength(0));
            foreach (var item in excelData)
            {
                users.Add(new Person(item[0], item[1], item[2], item[3]));
            }
            UserList = new ObservableCollection<Person>(users);
        }

    }
}
