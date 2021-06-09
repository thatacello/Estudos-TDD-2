using Store.Domain.Commands;

namespace Store.Domain.Handlers
{
    public interface IHandler<T> where T : ICommand // posso usar qualquer classe que faça uso de ICommand
    {
        ICommandResult Handle(T command);
    }
}