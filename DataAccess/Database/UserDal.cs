using System.Diagnostics;
using DataAccess.Context;
using Interface.Interface.Dal;
using Interface.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

public class UserDal(MyDbContext context) : IUserDal
{
    public List<UserModel> GetUsers()
    {
        Debug.WriteLine(context.Database.GetDbConnection().ConnectionString);
        return context.Users.ToList();
    }

    public void Register(UserModel userModel)
    {
        context.Users.Add(userModel);
        context.SaveChanges();
    }
    
    public bool UserNameExists(string userName)
    {
        return context.Users.Any(u => u.Name == userName);
    }
    
    public bool EmailExists(string email)
    {
        return context.Users.Any(u => u.Email == email);
    }
}