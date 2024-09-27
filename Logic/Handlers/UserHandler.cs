using Interface.Interface.Dal;
using Interface.Interface.Handlers;
using Interface.Models;

namespace Logic.Handlers;

public class UserHandler(IUserDal userDal) : IUserHandler
{
    public UserModel GetUser()
    {
        throw new NotImplementedException();
    }

    public List<UserModel> GetUsers()
    {
        return userDal.GetUsers();
    }
}