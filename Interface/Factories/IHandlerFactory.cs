using Interface.Interface.Handlers;

namespace Interface.Factories;

public interface IHandlerFactory
{
    public IUserHandler GetUserHandler();
    public IRouteHandler GetRouteHandler();
}