using Interface.Models;

namespace Interface.Interface.Handlers;

public interface IRouteHandler
{
    public Task<int> CreateRoute(int userId);
    public Task CreateDataPoints(DataPointModel[] dataPoints);
}