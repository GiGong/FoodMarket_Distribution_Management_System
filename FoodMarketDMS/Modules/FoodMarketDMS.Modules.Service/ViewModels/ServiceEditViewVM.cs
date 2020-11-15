using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.Service.ViewModels
{
    public class ServiceEditViewVM : DialogViewModelBase
    {

        private DateTime _serviceDate;
        private string _textServices;
        private string _textNote;

        private DelegateCommand _enterCommand;
        private DelegateCommand _closeCommand;

        public DateTime ServiceDate
        {
            get { return _serviceDate; }
            set { SetProperty(ref _serviceDate, value); }
        }

        public string TextServices
        {
            get { return _textServices; }
            set { SetProperty(ref _textServices, value); }
        }

        public string TextNote
        {
            get { return _textNote; }
            set { SetProperty(ref _textNote, value); }
        }


        public DelegateCommand EnterCommand =>
            _enterCommand ??= new DelegateCommand(ExecuteEnterCommand);

        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(ExecuteCloseCommand);


        public ServiceEditViewVM()
        {

        }


        public override void OnDialogOpened(IDialogParameters parameters)
        {
            var curr = parameters.GetValue<ServiceClass>(ServiceParameters.CURRENT_SERVICE);
            if (curr is null)
            {
                ServiceDate = DateTime.Now;
            }
            else
            {
                ServiceDate = curr.Date;
                TextServices = curr.Products;
                TextNote = curr.Note;
            }
        }

        private void ExecuteEnterCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK, 
                new DialogParameters { { ServiceParameters.RESULT_SERVICE, new ServiceClass(ServiceDate, TextServices, TextNote) } }));
        }

        private void ExecuteCloseCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

    }
}
