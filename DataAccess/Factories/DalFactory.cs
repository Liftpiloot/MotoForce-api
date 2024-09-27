using DataAccess.Context;
using DataAccess.Database;
using Interface.Factories;
using Interface.Interface.Dal;

namespace DataAccess.Factories;

public class DalFactory(MyDbContext context) : IDalFactory
{
    public IUserDal GetUserDal()
    {
        return new UserDal(context);
    }
}