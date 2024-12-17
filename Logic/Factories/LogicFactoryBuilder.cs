using Interface.Factories;

namespace Logic.Factories;

public class LogicFactoryBuilder(IDalFactory dalFactory) : ILogicFactoryBuilder
{
    public IHandlerFactory BuildHandlerFactory()
    {
        return new HandlerFactory(dalFactory);
    }
}