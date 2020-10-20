using Prism.Commands;

namespace FoodMarketDMS.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand LoadUserListCommand { get; }
        CompositeCommand SaveStateCommand { get; }
        CompositeCommand RegisterOfferCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand LoadUserListCommand { get; } = new CompositeCommand();
        public CompositeCommand SaveStateCommand { get; } = new CompositeCommand();
        public CompositeCommand RegisterOfferCommand { get; } = new CompositeCommand();
    }
}
