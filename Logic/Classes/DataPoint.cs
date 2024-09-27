using Interface.Models;

namespace Logic.Classes;

public class DataPoint
{
    public int Id { get; set; }

    public int RouteId { get; set; }

    public double Lat { get; set; }

    public double Lon { get; set; }

    public double? Lean { get; set; }

    public double? LateralG { get; set; }

    public double? Acceleration { get; set; }

    public DateTime Timestamp { get; set; }

    public double? Speed { get; set; }

    public Route Route { get; set; } = null!;
    
    public DataPoint(DataPointModel dataPointModel)
    {
        Id = dataPointModel.Id;
        RouteId = dataPointModel.RouteId;
        Lat = dataPointModel.Lat;
        Lon = dataPointModel.Lon;
        Lean = dataPointModel.Lean;
        LateralG = dataPointModel.LateralG;
        Acceleration = dataPointModel.Acceleration;
        Timestamp = dataPointModel.Timestamp;
        Speed = dataPointModel.Speed;
    }
}