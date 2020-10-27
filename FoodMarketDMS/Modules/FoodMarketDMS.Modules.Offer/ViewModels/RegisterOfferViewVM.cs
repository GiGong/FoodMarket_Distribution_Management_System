using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class RegisterOfferViewVM : DialogViewModelBase
    {
        private UserClass _selectedUser;
        private string _userName;
        private string _resultText;
        private ObservableCollection<UserClass> _userSearchList;
        private string _provider;
        private string _textOfferItems;
        private string _textServiceItems;

        private List<UserClass> _users;


        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;
        private DelegateCommand<string> _userSearchCommand;


        public UserClass SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string ResultText
        {
            get { return _resultText; }
            set { SetProperty(ref _resultText, value); }
        }

        public ObservableCollection<UserClass> UserSearchList
        {
            get { return _userSearchList; }
            set { SetProperty(ref _userSearchList, value); }
        }

        public string Provider
        {
            get { return _provider; }
            set { SetProperty(ref _provider, value); }
        }

        public string TextOfferItems
        {
            get { return _textOfferItems; }
            set { SetProperty(ref _textOfferItems, value); }
        }

        public string TextServiceItems
        {
            get { return _textServiceItems; }
            set { SetProperty(ref _textServiceItems, value); }
        }


        public DelegateCommand EnterCommand =>
            _enterCommand ??= new DelegateCommand(ExecuteEnterCommand);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(ExecuteCloseCommand);

        public DelegateCommand<string> UserSearchCommand =>
            _userSearchCommand ??= new DelegateCommand<string>(ExecuteUserSearchCommand);


        public RegisterOfferViewVM()
        {
            UserSearchList = new ObservableCollection<UserClass>();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            _users = parameters.GetValue<List<UserClass>>(OfferParameters.Users);
        }

        private void ExecuteEnterCommand()
        {
            List<string> offerItems = string.IsNullOrWhiteSpace(TextOfferItems) ? null : new List<string>(TextOfferItems.Split('\n'));
            List<string> serviceItems = string.IsNullOrWhiteSpace(TextServiceItems) ? null : new List<string>(TextServiceItems.Split('\n'));

            offerItems?.RemoveEmptyString();
            offerItems?.TrimString();
            serviceItems?.RemoveEmptyString();
            serviceItems?.TrimString();

            RaiseRequestClose(new DialogResult(ButtonResult.OK,
                new DialogParameters
                {
                    { OfferParameters.UserId, SelectedUser }, // TODO: OfferUser -> UserId
                    { OfferParameters.UserName, UserName },
                    { OfferParameters.Provider, Provider },
                    { OfferParameters.OfferItems, offerItems },
                    { OfferParameters.ServiceItems, serviceItems }
                }));
        }

        private void ExecuteCloseCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private void ExecuteUserSearchCommand(string userName)
        {
            var searchResult = _users.FindAll(user => user.Name == userName);
            if (searchResult is null)
            {
                return;
            }

            UserSearchList.AddRange(searchResult);
        }


    }
}
