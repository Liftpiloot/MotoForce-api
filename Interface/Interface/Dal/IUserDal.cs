using Interface.Models;

namespace Interface.Interface.Dal;

public interface IUserDal
{
    public List<UserModel> GetUsers();
    public bool Register(UserModel userModel);
    public bool UserNameExists(string userName);
    public bool EmailExists(string email);
}