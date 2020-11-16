using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserEditViewVM : DialogViewModelBase
    {
        private UserClass _currentUser;

        private string _name;
        private string _birth;
        private string _phoneNumber;


        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Birth
        {
            get { return _birth; }
            set { SetProperty(ref _birth, value); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }


        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;


        public DelegateCommand EnterCommand =>
            _enterCommand ??= new DelegateCommand(ExecuteEnterCommand);

        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(ExecuteCloseCommand);


        public UserEditViewVM()
        {
            _currentUser = null;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (!(parameters is null))
            {
                _currentUser = parameters.GetValue<UserClass>(UserParameters.CURRENT_USER);
                Name = _currentUser.Name;
                Birth = _currentUser.Birth;
                PhoneNumber = _currentUser.PhoneNumber;
            }
        }

        private void ExecuteEnterCommand()
        {
            UserClass resultUser;
            if (!(_currentUser is null))
            {
                _currentUser.Name = Name;
                _currentUser.Birth = Birth;
                _currentUser.PhoneNumber = PhoneNumber;
                resultUser = _currentUser;
            }
            else
            {
                resultUser = new UserClass(Name, Birth, PhoneNumber);
            }

            RaiseRequestClose(new DialogResult(ButtonResult.OK,
                new DialogParameters { { UserParameters.RESULT_USER, resultUser } }));
        }

        private void ExecuteCloseCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

    }
}
