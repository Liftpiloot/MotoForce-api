using Interface.Interface.Dal;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Containers;

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

    public void Register(UserModel userModel)
    {
        UserContainer userContainer = new UserContainer(userDal);
        userContainer.Register(userModel);
    }

    public UserModel? Login(string identifier)
    {
        return userDal.Login(identifier);
    }
}