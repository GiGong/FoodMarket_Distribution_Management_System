using FoodMarketDMS.Modules.User.ViewModels;
using FoodMarketDMS.Modules.User.Views;
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
            containerRegistry.RegisterSingleton<IUserModuleCommands, UserModuleCommands>();

            containerRegistry.RegisterForNavigation<UserListView, UserListViewVM>();
            containerRegistry.RegisterForNavigation<UserMenuView, UserMenuViewVM>();

            containerRegistry.RegisterDialog<UserEditView, UserEditViewVM>();
        }
    }
}