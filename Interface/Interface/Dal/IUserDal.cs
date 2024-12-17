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
}