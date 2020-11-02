using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Core.Events;
using FoodMarketDMS.Modules.Offer.Views;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Threading;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class OfferListViewVM : BindableBase
    {
        private int _totalUser;

        private double _usageRate;
        private int _todayUser;
        private ObservableCollection<OfferClass> _offerList;

        private readonly IDialogService _dialogService;
        private readonly IStateWrapperService _stateWrapperService;

        private DelegateCommand _registerOfferCommand;


        public double UsageRate
        {
            get { return _usageRate; }
            set { SetProperty(ref _usageRate, value); }
        }

        public int TodayUser
        {
            get { return _todayUser; }
            set
            {
                SetProperty(ref _todayUser, value);
                if (_totalUser != 0)
                {
                    UsageRate = (double)TodayUser / _totalUser * 100;
                }
            }
        }

        public ObservableCollection<OfferClass> OfferList
        {
            get { return _offerList; }
            set { SetProperty(ref _offerList, value); }
        }

        public DelegateCommand RegisterOfferCommand =>
            _registerOfferCommand ??= new DelegateCommand(ExecuteRegisterOfferCommand);


        public OfferListViewVM(IDialogService dialogService, IEventAggregator eventAggregator, IApplicationCommands applicationCommands,
                                IStateWrapperService stateWrapperService)
        {
            _dialogService = dialogService;
            _stateWrapperService = stateWrapperService;

            eventAggregator.GetEvent<UserCountChanged>().Subscribe(UserCount_Changed);
            eventAggregator.GetEvent<UserListChanged>().Subscribe(UserList_Changed);
            applicationCommands.RegisterOfferCommand.RegisterCommand(RegisterOfferCommand);

            OfferList = new ObservableCollection<OfferClass>(_stateWrapperService.Offers);
            OfferList.CollectionChanged += OfferList_CollectionChanged;

            _totalUser = 0;
            UsageRate = 0;
            TodayUser = OfferList.Count(item => item.Date.Date == DateTime.Now.Date);
        }

        private void OfferList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (OfferClass item in e.NewItems)
                    {
                        _stateWrapperService.Offers.Add(item);
                        if (item.Date.Date == DateTime.Now.Date)
                        {
                            TodayUser++;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (OfferClass item in e.OldItems)
                    {
                        _stateWrapperService.Offers.Remove(item);
                        if (item.Date == DateTime.Now)
                        {
                            TodayUser--;
                        }
                    }
                    break;
            }
        }

        private void UserCount_Changed(int count)
        {
            _totalUser = count;
            UsageRate = (double)TodayUser / _totalUser * 100;
        }

        private void UserList_Changed()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
                () =>
                {
                    OfferList.Clear();
                    _stateWrapperService.Offers.Clear();
                });
        }

        private void ExecuteRegisterOfferCommand()
        {
            _dialogService.ShowDialog(nameof(RegisterOfferView), new DialogParameters { { OfferParameters.Users, _stateWrapperService.Users } },
                (result) =>
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    OfferList.Add(result.Parameters.GetValue<OfferClass>(OfferParameters.NewOffer));
                });
        }
    }
}
