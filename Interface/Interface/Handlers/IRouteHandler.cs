using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IRouteHandler
{
    public Task<int> CreateRoute(RouteModel route);
    public Task CreateDataPoints(DataPointModel[] dataPoints);
}