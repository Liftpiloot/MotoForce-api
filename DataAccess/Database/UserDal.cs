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
}