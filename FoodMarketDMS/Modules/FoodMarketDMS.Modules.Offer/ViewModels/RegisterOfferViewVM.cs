using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class RegisterOfferViewVM : DialogViewModelBase
    {
        private UserClass _offerUser;
        private string _userName;
        private string _provider;
        private string _textOfferItems;
        private string _textServiceItems;

        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;


        public DelegateCommand EnterCommand =>
            _enterCommand ??= new DelegateCommand(ExecuteEnterCommand);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(ExecuteCloseCommand);


        public UserClass OfferUser
        {
            get { return _offerUser; }
            set { SetProperty(ref _offerUser, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
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


        public RegisterOfferViewVM()
        {
            // TODO: 제공 등록 Dialog 개발 시작
        }

        private void ExecuteEnterCommand()
        {
            List<string> offerItems = string.IsNullOrWhiteSpace(TextOfferItems) ? null : new List<string>(TextOfferItems.Split('\n'));
            List<string> serviceItems = string.IsNullOrWhiteSpace(TextServiceItems) ? null : new List<string>(TextServiceItems.Split('\n'));

            offerItems?.RemoveEmptyString();
            serviceItems?.RemoveEmptyString();

            RaiseRequestClose(new DialogResult(ButtonResult.OK,
                new DialogParameters
                {
                    { OfferParameters.OfferUser, OfferUser },
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

    }
}
