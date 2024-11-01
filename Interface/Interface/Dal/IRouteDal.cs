using Interface.Models;

namespace Interface.Interface.Dal;

public interface IRouteDal
{
    public Task<int> CreateRoute(int userId);
    public Task CreateDataPoint(DataPointModel[] dataPoints);
}