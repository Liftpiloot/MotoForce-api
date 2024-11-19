using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IRouteHandler
{
    public Task<int> CreateRoute(int userId);
    public Task CreateDataPoints(DataPointModel[] dataPoints);
    public Task<double> GetMaxSpeed(int routeId);
    public Task<double> GetMaxLean(int routeId);
    public Task<double> GetMaxG(int routeId);
    public Task<RouteModel> GetRoute(int routeId);
    public Task DeleteRoute(int routeId);
}