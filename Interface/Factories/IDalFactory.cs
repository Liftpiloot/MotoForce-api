using Interface.Interface.Dal;

namespace Interface.Factories;

public interface IDalFactory
{
    public IUserDal GetUserDal();
}