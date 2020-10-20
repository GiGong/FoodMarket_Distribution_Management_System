using FoodMarketDMS.Modules.Offer.ViewModels;
using FoodMarketDMS.Modules.Offer.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace FoodMarketDMS.Modules.Offer
{
    public class OfferModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<OfferListView, OfferListViewVM>();
            containerRegistry.RegisterForNavigation<OfferMenuView, OfferMenuViewVM>();

            containerRegistry.RegisterDialog<RegisterOfferView, RegisterOfferViewVM>();
        }
    }
}