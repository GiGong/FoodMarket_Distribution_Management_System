using FoodMarketDMS.Core;
using Prism.Mvvm;

namespace FoodMarketDMS.Modules.Service.ViewModels
{
    public class ServiceMenuViewVM : BindableBase
    {
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public ServiceMenuViewVM(IApplicationCommands applicationCommands)
        {
            ApplicationCommands = applicationCommands;
        }
    }
}
