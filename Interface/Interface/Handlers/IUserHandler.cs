using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IUserHandler
{
    public UserModel GetUser();
    public List<UserModel> GetUsers();
    public void Register(UserModel userModel);
}