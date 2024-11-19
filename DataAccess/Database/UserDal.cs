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

    public async Task<UserModel?> Login(string identifier)
    {
        // return user where name or email is equal to identifier
        return await context.Users.FirstOrDefaultAsync(u => u.Name == identifier || u.Email == identifier);
    }

    public async Task<double> GetMaxSpeed(int userId)
    {
        var allRoutes = await context.Routes.Include(r => r.DataPoints).Where(r => r.UserId == userId).ToListAsync();
        if (allRoutes.Count == 0)
        {
            throw new Exception("No routes found");
        }
        var allDataPoints = allRoutes.SelectMany(r => r.DataPoints).ToList();
        if (allDataPoints.Count == 0)
        {
            throw new Exception("No data points found");
        }
        return (double)allDataPoints.Max(dp => dp.Speed);
    }
}