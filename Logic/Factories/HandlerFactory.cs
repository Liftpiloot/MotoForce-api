using Interface.Factories;
using Interface.Interface.Handlers;
using Logic.Handlers;

namespace Logic.Factories;

public class HandlerFactory(IDalFactory dalFactory) : IHandlerFactory
{
    public IUserHandler GetUserHandler()
    {
        return new UserHandler(dalFactory.GetUserDal());
    }
}