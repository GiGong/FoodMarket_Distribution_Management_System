using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Extensions;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class RegisterOfferViewVM : DialogViewModelBase
    {
        private UserClass _selectedUser;
        private string _userName;
        private string _textResult;
        private ObservableCollection<UserClass> _userSearchList;
        private bool _isSearchExist;
        private string _provider;
        private string _textOfferItems;
        private string _textServiceItems;

        private List<UserClass> _currUsers;


        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;
        private DelegateCommand<string> _userSearchCommand;


        public UserClass SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                SetProperty(ref _selectedUser, value);
                TextResult = SelectedUser is null ? string.Empty : $"[{SelectedUser.Birth}] [{SelectedUser.Name}] [{SelectedUser.PhoneNumber}]";
            }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string TextResult
        {
            get { return _textResult; }
            set { SetProperty(ref _textResult, value); }
        }

        public ObservableCollection<UserClass> UserSearchList
        {
            get { return _userSearchList; }
            set { SetProperty(ref _userSearchList, value); }
        }

        public bool IsSearchExist
        {
            get { return _isSearchExist; }
            set { SetProperty(ref _isSearchExist, value); }
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
            _currUsers = parameters.GetValue<List<UserClass>>(OfferParameters.Users);
        }

        private void ExecuteEnterCommand()
        {
            var searchResult = SearchUsers(UserName);

            if (searchResult.Count < 1)
            {
                // if can add unexist user to offer list
                //var result = MessageBox.Show($"\"{UserName}\" 이/가 이용자 목록에 존재하지 않습니다.\n등록하시겠습니까?", (string)Application.Current.Resources["Program_Name"], MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                //{
                //    RaiseRequestClose(new DialogResult(ButtonResult.OK, MakeParameter(null)));
                //}
                MessageBox.Show($"\"{UserName}\" 이/가 이용자 목록에 존재하지 않습니다.", (string)Application.Current.Resources["Program_Name"], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (searchResult.Count > 1 && SelectedUser is null)
            {
                MessageBox.Show("중복된 이용자가 있습니다.", (string)Application.Current.Resources["Program_Name"], MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RaiseRequestClose(new DialogResult(ButtonResult.OK, MakeParameter(searchResult)));
        }

        private void ExecuteCloseCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private void ExecuteUserSearchCommand(string userName)
        {
            SelectedUser = null;
            UserSearchList.Clear();
            UserSearchList.AddRange(SearchUsers(userName));
        }

        private DialogParameters MakeParameter(List<UserClass> searchUsers)
        {
            List<string> offerItems = string.IsNullOrWhiteSpace(TextOfferItems) ? new List<string>() : new List<string>(TextOfferItems.Split('\n'));
            List<string> serviceItems = string.IsNullOrWhiteSpace(TextServiceItems) ? new List<string>() : new List<string>(TextServiceItems.Split('\n'));

            offerItems?.RemoveEmptyString();
            offerItems?.TrimString();
            serviceItems?.RemoveEmptyString();
            serviceItems?.TrimString();

            UserClass offerUser = searchUsers[0];
            return new DialogParameters { { OfferParameters.NewOffer, new OfferClass(DateTime.Now, offerUser, Provider, offerItems, serviceItems) } };
            // if can add unexist user to offer list
            //UserClass offerUser = searchUsers?[0];
            //return new DialogParameters { { OfferParameters.NewOffer, new OfferClass(DateTime.Now, offerUser?.Id ?? -1, offerUser?.Name ?? UserName, Provider, offerItems, serviceItems) } };
        }

        private List<UserClass> SearchUsers(string name)
        {
            var result = _currUsers.FindAll(user => user.Name == name);
            IsSearchExist = result.Count > 0;
            return result;
        }
    }
}
