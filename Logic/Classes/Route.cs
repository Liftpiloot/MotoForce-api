using Interface.Models;

namespace Logic.Classes;

public class Route
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Distance { get; set; }

    public List<DataPoint> DataPoints { get; set; }

    public User User { get; set; } = null!;
    
    public Route(RouteModel routeModel)
    {
        Id = routeModel.Id;
        UserId = routeModel.UserId;
        Distance = routeModel.Distance;
        DataPoints = routeModel.DataPoints.Select(dp => new DataPoint(dp)).ToList();
    }
}