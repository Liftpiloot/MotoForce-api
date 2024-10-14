using Interface.Models;

namespace Interface.Interface.Dal;

public interface IRouteDal
{
    public Task<int> CreateRoute(RouteModel route);
    public Task CreateDataPoint(DataPointModel[] dataPoints);
}