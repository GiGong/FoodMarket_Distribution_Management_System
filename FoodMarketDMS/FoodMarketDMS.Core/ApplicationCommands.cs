using Prism.Commands;

namespace FoodMarketDMS.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand RegisterOfferCommand { get; }
        CompositeCommand RegisterServiceCommand { get; }
        CompositeCommand EditServiceCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand RegisterOfferCommand { get; } = new CompositeCommand();
        public CompositeCommand RegisterServiceCommand { get; } = new CompositeCommand();
        public CompositeCommand EditServiceCommand { get; } = new CompositeCommand();
    }
}
