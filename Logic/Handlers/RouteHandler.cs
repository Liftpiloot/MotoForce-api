using Interface.Interface.Dal;
using Interface.Interface.Handlers;
using Interface.Models;

namespace Logic.Handlers;

public class RouteHandler(IRouteDal routeDal) : IRouteHandler
{
    public async Task<int> CreateRoute(int userId)
    {
        return await routeDal.CreateRoute(userId);
    }

    public async Task CreateDataPoints(DataPointModel[] dataPoints)
    {
        await routeDal.CreateDataPoint(dataPoints);
    }
}