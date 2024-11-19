using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IUserHandler
{
    public UserModel GetUser();
    public List<UserModel> GetUsers();
    public void Register(UserModel userModel);
    public Task<UserModel?> Login(string identifier);
    public Task<double> GetMaxSpeed(int userId);
    public Task<double> GetMaxLean(int userId);
    public Task<double> GetMaxG(int userId);
}