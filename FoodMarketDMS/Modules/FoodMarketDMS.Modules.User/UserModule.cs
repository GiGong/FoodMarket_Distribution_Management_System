using FoodMarketDMS.Modules.User.ViewModels;
using FoodMarketDMS.Modules.User.Views;
using FoodMarketDMS.Services;
using FoodMarketDMS.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace FoodMarketDMS.Modules.User
{
    public class UserModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IExcelService, ExcelService>();

            containerRegistry.RegisterForNavigation<UserListView, UserListViewVM>();
            containerRegistry.RegisterForNavigation<UserMenuView, UserMenuViewVM>();
        }
    }
}