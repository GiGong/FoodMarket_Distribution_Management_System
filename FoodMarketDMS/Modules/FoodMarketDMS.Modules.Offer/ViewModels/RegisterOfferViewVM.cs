using FoodMarketDMS.Core;
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
        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;


        public DelegateCommand EnterCommand =>
            _enterCommand ??= new DelegateCommand(ExecuteEnterCommand);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(ExecuteCloseCommand);


        public RegisterOfferViewVM()
        {
            // TODO: 제공 등록 Dialog 개발 시작
        }

        private void ExecuteEnterCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { }));
        }

        private void ExecuteCloseCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

    }
}
