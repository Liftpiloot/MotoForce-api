using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IUserHandler
{
    public Task<UserModel> GetUser(int userId);
    public List<UserModel> GetUsers();
    public void Register(UserModel userModel);
    public Task<UserModel?> Login(string identifier);
    public Task<double> GetMaxSpeed(int userId);
    public Task<double> GetMaxLean(int userId);
    public Task<double> GetMaxG(int userId);
    public Task<List<RouteModel>> GetRoutes(int userId, int count);
    public Task<List<UserModel>> GetFriends(int userId);
    public Task AddFriend(int userId, string email);
    public Task AcceptFriend(int friendId);
    public Task<List<FriendModel>> GetFriendRequests(int userId);
    public Task<UserModel> getUserByEmail(string email);
    public Task DenyFriend(int friendId);
    public Task RemoveFriend(int userId, int friendId);
}