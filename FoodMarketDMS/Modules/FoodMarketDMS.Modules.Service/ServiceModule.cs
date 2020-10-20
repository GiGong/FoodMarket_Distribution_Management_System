using FoodMarketDMS.Modules.Service.ViewModels;
using FoodMarketDMS.Modules.Service.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace FoodMarketDMS.Modules.Service
{
    public class ServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ServiceListView, ServiceListViewVM>();
            containerRegistry.RegisterForNavigation<ServiceMenuView, ServiceMenuViewVM>();
        }
    }
}