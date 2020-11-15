using Prism.Commands;

namespace FoodMarketDMS.Modules.User
{
    public interface IUserModuleCommands
    {
        CompositeCommand LoadUserListCommand { get; }
        CompositeCommand AddUserCommand { get; }
    }

    public class UserModuleCommands : IUserModuleCommands
    {
        public CompositeCommand LoadUserListCommand { get; } = new CompositeCommand();
        public CompositeCommand AddUserCommand { get; } = new CompositeCommand();
    }
}
