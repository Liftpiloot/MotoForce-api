using Interface.Models;

namespace Interface.Interface.Dal;

public interface IUserDal
{
    public List<UserModel> GetUsers();
}