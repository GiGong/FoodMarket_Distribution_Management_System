using Prism.Commands;

namespace FoodMarketDMS.Modules.Offer
{
    public interface IOfferModuleCommands
    {
        CompositeCommand SearchOfferCommand { get; }
    }

    public class OfferModuleCommands : IOfferModuleCommands
    {
        public CompositeCommand SearchOfferCommand { get; } = new CompositeCommand();
    }
}
