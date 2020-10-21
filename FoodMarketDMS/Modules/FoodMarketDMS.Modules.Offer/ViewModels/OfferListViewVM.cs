using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Modules.Offer.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class OfferListViewVM : BindableBase
    {

        private double _usageRate;
        private int _todayUser;
        private ObservableCollection<OfferClass> _offerList;

        private readonly IDialogService _dialogService;

        private DelegateCommand _registerOfferCommand;


        public DelegateCommand RegisterOfferCommand =>
            _registerOfferCommand ??= new DelegateCommand(ExecuteRegisterOfferCommand);

        public double UsageRate
        {
            get { return _usageRate; }
            set { SetProperty(ref _usageRate, value); }
        }

        public int TodayUser
        {
            get { return _todayUser; }
            set { SetProperty(ref _todayUser, value); }
        }

        public ObservableCollection<OfferClass> OfferList
        {
            get { return _offerList; }
            set { SetProperty(ref _offerList, value); }
        }


        public OfferListViewVM(IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _dialogService = dialogService;

            applicationCommands.RegisterOfferCommand.RegisterCommand(RegisterOfferCommand);

            OfferList = new ObservableCollection<OfferClass>();
            OfferList.CollectionChanged += OfferList_CollectionChanged;
        }

        private void OfferList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (OfferClass item in e.NewItems)
                    {
                        if (item.Date == DateTime.Now)
                        {
                            TodayUser++;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (OfferClass item in e.OldItems)
                    {
                        if (item.Date == DateTime.Now)
                        {
                            TodayUser--;
                        }
                    }
                    break;
            }
        }

        private void ExecuteRegisterOfferCommand()
        {
            _dialogService.ShowDialog(nameof(RegisterOfferView), null,
                (result) =>
                {
                    if (result == null || result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    OfferList.Add(new OfferClass(
                        DateTime.Now,
                        result.Parameters.GetValue<UserClass>(OfferParameters.OfferUser),
                        result.Parameters.GetValue<string>(OfferParameters.UserName),
                        result.Parameters.GetValue<string>(OfferParameters.Provider),
                        result.Parameters.GetValue<List<string>>(OfferParameters.OfferItems),
                        result.Parameters.GetValue<List<string>>(OfferParameters.ServiceItems)
                    ));
                });
        }
    }
}
