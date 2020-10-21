using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserMenuViewVM : BindableBase
    {
        private IApplicationCommands _applicationCommands;

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public UserMenuViewVM(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
        }
    }
}
