using System.Windows.Input;

namespace UpCare.Helpers
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }
    }
}
