using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace FoodMarketDMS.Core
{
    public class DialogViewModelBase : BindableBase, IDialogAware
    {
        private string _iconSource;
        private string _title;

        public string IconSource
        {
            get { return _iconSource; }
            set { SetProperty(ref _iconSource, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public DialogViewModelBase()
        {
            Title = (string)Application.Current.Resources["Program_Name"];
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }
    }
}
