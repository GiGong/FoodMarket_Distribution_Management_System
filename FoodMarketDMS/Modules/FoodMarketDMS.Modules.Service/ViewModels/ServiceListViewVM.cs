using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using FoodMarketDMS.Modules.Service.Views;
using FoodMarketDMS.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodMarketDMS.Modules.Service.ViewModels
{
    public class ServiceListViewVM : BindableBase
    {
        private ServiceClass _selectedService;
        private ObservableCollection<ServiceClass> _serviceList;

        private readonly IDialogService _dialogService;
        private readonly IStateWrapperService _stateWrapperService;

        private DelegateCommand _registerServiceCommand;
        private DelegateCommand _editServiceCommand;


        public ServiceClass SelectedService
        {
            get { return _selectedService; }
            set { SetProperty(ref _selectedService, value); }
        }

        public ObservableCollection<ServiceClass> ServiceList
        {
            get { return _serviceList; }
            set { SetProperty(ref _serviceList, value); }
        }

        public DelegateCommand RegisterServiceCommand =>
            _registerServiceCommand ??= new DelegateCommand(ExecuteRegisterServiceCommand);

        public DelegateCommand EditServiceCommand =>
            _editServiceCommand ??= new DelegateCommand(ExecuteEditServiceCommand, CanExecuteEditServiceCommand).ObservesProperty(() => SelectedService);

        public ServiceListViewVM(IDialogService dialogService, IApplicationCommands applicationCommands, IStateWrapperService stateWrapperService)
        {
            _dialogService = dialogService;
            _stateWrapperService = stateWrapperService;

            applicationCommands.RegisterServiceCommand.RegisterCommand(RegisterServiceCommand);
            applicationCommands.EditServiceCommand.RegisterCommand(EditServiceCommand);

            ServiceList = new ObservableCollection<ServiceClass>(_stateWrapperService.Services);
            ServiceList.CollectionChanged += ServiceList_CollectionChanged;
        }

        private void ServiceList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    
                    _stateWrapperService.Services.AddRange(e.NewItems.Cast<ServiceClass>());
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    _stateWrapperService.Services[e.OldStartingIndex] = ServiceList[e.NewStartingIndex];
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (ServiceClass item in e.OldItems)
                    {
                        _stateWrapperService.Services.Remove(item);
                    }
                    break;
            }
        }

        private void ExecuteRegisterServiceCommand()
        {
            _dialogService.ShowDialog(nameof(ServiceEditView), new DialogParameters { { ServiceParameters.CURRENT_SERVICE, null } },
                (result) =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        ServiceList.Add(result.Parameters.GetValue<ServiceClass>(ServiceParameters.RESULT_SERVICE));
                    }
                });
        }

        private bool CanExecuteEditServiceCommand()
        {
            return !(SelectedService is null);
        }

        private void ExecuteEditServiceCommand()
        {
            int index = ServiceList.IndexOf(SelectedService);
            _dialogService.ShowDialog(nameof(ServiceEditView), new DialogParameters { { ServiceParameters.CURRENT_SERVICE, SelectedService } },
            (result) =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    ServiceList[index] = result.Parameters.GetValue<ServiceClass>(ServiceParameters.RESULT_SERVICE);
                }
            });
        }

    }
}
