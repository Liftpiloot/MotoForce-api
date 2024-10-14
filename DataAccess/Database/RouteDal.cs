using DataAccess.Context;
using Interface.Interface.Dal;
using Interface.Models;

namespace DataAccess.Database;

public class RouteDal(MyDbContext context) : IRouteDal
{
    public async Task<int> CreateRoute(RouteModel route)
    {
        context.Routes.Add(route);
        await context.SaveChangesAsync();
        return route.Id;
    }

    public async Task CreateDataPoint(DataPointModel[] dataPoints)
    {
        context.DataPoints.AddRange(dataPoints);
        await context.SaveChangesAsync();
    }
}