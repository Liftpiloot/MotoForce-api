using Interface.Interface.Dal;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Containers;

namespace Logic.Handlers;

public class UserHandler(IUserDal userDal) : IUserHandler
{
    public async Task<UserModel> GetUser(int userId)
    {
        return await userDal.GetUser(userId);
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

    public async Task<List<UserModel>> GetFriends(int userId)
    {
        return await userDal.GetFriends(userId);
    }

    public async Task AddFriend(int userId, string email)
    {
        await userDal.AddFriend(userId, email);
    }

    public async Task AcceptFriend(int friendId)
    {
        await userDal.AcceptFriend(friendId);
    }

    public async Task<List<FriendModel>> GetFriendRequests(int userId)
    {
        return await userDal.GetFriendRequests(userId);
    }

    public async Task<UserModel> getUserByEmail(string email)
    {
        return await userDal.getUserByEmail(email);
    }

    public async Task DenyFriend(int friendId)
    {
        await userDal.DenyFriend(friendId);
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        await userDal.RemoveFriend(userId, friendId);
    }
}