using System.Windows.Input;

namespace UpCare.Helpers
{
    public interface IMediator
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
