using FoodMarketDMS.Core;
using FoodMarketDMS.Modules.Offer.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class OfferListViewVM : BindableBase
    {//TODO: 이용률, 오늘 이용자
        private readonly IDialogService _dialogService;

        private DelegateCommand _registerOfferCommand;


        public DelegateCommand RegisterOfferCommand =>
            _registerOfferCommand ??= new DelegateCommand(ExecuteRegisterOfferCommand);


        public OfferListViewVM(IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _dialogService = dialogService;
            applicationCommands.RegisterOfferCommand.RegisterCommand(RegisterOfferCommand);
        }

        private void ExecuteRegisterOfferCommand()
        {
            _dialogService.ShowDialog(nameof(RegisterOfferView), new DialogParameters { }, (r) => { });
        }
    }
}
