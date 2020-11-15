using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserEditViewVM : DialogViewModelBase
    {


        public UserEditViewVM()
        {

        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters is null)
            {

            }
        }
    }
}
