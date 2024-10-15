using DataAccess.Context;
using Interface.Interface.Dal;
using Interface.Models;

namespace DataAccess.Database;

public class RouteDal(MyDbContext context) : IRouteDal
{
    public async Task<int> CreateRoute(int userId)
    {
        var userModel = await context.Users.FindAsync(userId);
        if (userModel == null)
        {
            throw new Exception("User not found");
        }
        var route = new RouteModel
        {
            UserId = userId,
            UserModel = userModel,
        };
        context.Routes.Add(route);
        await context.SaveChangesAsync();
        return route.Id;
    }

    public async Task CreateDataPoint(DataPointModel[] dataPoints)
    {
        var route = await context.Routes.FindAsync(dataPoints[0].RouteId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }
        dataPoints.ToList().ForEach(x => x.RouteModel = route);
        context.DataPoints.AddRange(dataPoints);
        await context.SaveChangesAsync();
    }
}