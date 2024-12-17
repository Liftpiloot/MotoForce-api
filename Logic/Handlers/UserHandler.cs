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

    public async Task<UserModel?> Login(string identifier)
    {
        return await userDal.Login(identifier);
    }

    public async Task<double> GetMaxSpeed(int userId)
    {
        return await userDal.GetMaxSpeed(userId);
    }

    public async Task<double> GetMaxLean(int userId)
    {
        return await userDal.GetMaxLean(userId);
    }

    public async Task<double> GetMaxG(int userId)
    {
        return await userDal.GetMaxG(userId);
    }

    public Task<List<RouteModel>> GetRoutes(int userId, int count)
    {
        return userDal.GetRoutes(userId, count);
    }
}