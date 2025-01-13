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

    public async Task<double> GetMaxLean(int userId)
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
        return (double)allDataPoints.Max(dp => dp.Lean);
    }

    public async Task<double> GetMaxG(int userId)
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
        return double.Max((double)allDataPoints.Max(dp => dp.LateralG), (double)allDataPoints.Max(dp => dp.Acceleration));
    }

    public async Task<List<RouteModel>> GetRoutes(int userId, int count)
    {
        var routes = await context.Routes.Include(r => r.DataPoints).Where(r => r.UserId == userId).OrderByDescending(r => r.Id).Take(count).ToListAsync(); 
        if (routes.Count == 0)
        {
            throw new Exception("No routes found");
        }
        return routes;
    }

    public async Task<List<UserModel>> GetFriends(int userId)
    {
        var userSendRequest = await context.Friends.Where(f => f.SenderId == userId && f.Accepted).Select(f => f.ReceiverId).ToListAsync();
        var userReceiveRequest = await context.Friends.Where(f => f.ReceiverId == userId && f.Accepted).Select(f => f.SenderId).ToListAsync();
        var friends = await context.Users.Where(u => userSendRequest.Contains(u.Id) || userReceiveRequest.Contains(u.Id)).ToListAsync();
        return friends;
    }

    public async Task AddFriend(int userId, string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var friend = new FriendModel
        {
            SenderId = userId,
            ReceiverId = user.Id,
            Accepted = false,
            CreatedAt = DateTime.Now
        };
        context.Friends.Add(friend);
        await context.SaveChangesAsync();
    }

    public async Task AcceptFriend(int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.Id == friendId);
        if (friend == null)
        {
            throw new Exception("Friend request not found");
        }
        friend.Accepted = true;
        await context.SaveChangesAsync();
    }

    public async Task<List<FriendModel>> GetFriendRequests(int userId)
    {
        var friendRequests = await context.Friends.Where(f => f.ReceiverId == userId && !f.Accepted).ToListAsync();
        return friendRequests;
    }

    public async Task<UserModel> GetUser(int userId)
    {
        UserModel user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }

    public async Task<UserModel> getUserByEmail(string email)
    {
        UserModel user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }

    public async Task DenyFriend(int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.Id == friendId);
        if (friend == null)
        {
            throw new Exception("Friend request not found");
        }
        context.Friends.Remove(friend);
        await context.SaveChangesAsync();
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.SenderId == userId && f.ReceiverId == friendId || f.SenderId == friendId && f.ReceiverId == userId);
        if (friend == null)
        {
            throw new Exception("Friend not found");
        }
        context.Friends.Remove(friend);
        await context.SaveChangesAsync();
    }
}