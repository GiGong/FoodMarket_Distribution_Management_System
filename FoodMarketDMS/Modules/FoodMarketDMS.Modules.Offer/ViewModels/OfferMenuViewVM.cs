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
        private readonly IOfferModuleCommands _offerModuleCommands;


        private DelegateCommand _searchCommand;


        public DelegateCommand SearchCommand =>
            _searchCommand ??= new DelegateCommand(ExecuteSearchCommand);


        public OfferMenuViewVM(IOfferModuleCommands offerModuleCommands)
        {
            _offerModuleCommands = offerModuleCommands;
        }

        private void ExecuteSearchCommand()
        {
            // TODO: 검색 dialog -> 검색 -> _offerModuleCommands.SearchOfferCommand 에 전달 -> offer list view VM에서 search 실행 -> selected item 에 반영
        }
    }
}
