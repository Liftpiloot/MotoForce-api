using Interface.Models;

namespace Interface.Interface.Dal;

public interface IUserDal
{
    public List<UserModel> GetUsers();
    public void Register(UserModel userModel);
    public bool UserNameExists(string userName);
    public bool EmailExists(string email);
    public Task<UserModel?> Login(string identifier);
    public Task<double> GetMaxSpeed(int userId);
    public Task<double> GetMaxLean(int userId);
    public Task<double> GetMaxG(int userId);
    public Task<List<RouteModel>> GetRoutes(int userId, int count);
    public Task<List<UserModel>> GetFriends(int userId);
    public Task AddFriend(int userId, string email);
    public Task AcceptFriend(int friendId);
    public Task<List<FriendModel>> GetFriendRequests(int userId);
    public Task<UserModel> GetUser(int userId);
    public Task<UserModel> getUserByEmail(string email);
    public Task DenyFriend(int friendId);
    public Task RemoveFriend(int userId, int friendId);
}