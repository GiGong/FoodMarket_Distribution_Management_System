using FoodMarketDMS.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Modules.Offer.ViewModels
{
    public class OfferMenuViewVM : BindableBase
    {
        private IApplicationCommands _applicationCommands;

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public OfferMenuViewVM(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
        }
    }
}
