using Prism.Mvvm;

namespace FoodMarketDMS.Modules.User.ViewModels
{
    public class UserMenuViewVM : BindableBase
    {
        private IUserModuleCommands _userModuleCommands;

        public IUserModuleCommands UserModuleCommands
        {
            get { return _userModuleCommands; }
            set { SetProperty(ref _userModuleCommands, value); }
        }

        public UserMenuViewVM(IUserModuleCommands userModuleCommands)
        {
            UserModuleCommands = userModuleCommands;
        }
    }
}
