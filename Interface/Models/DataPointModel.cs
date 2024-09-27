namespace Interface.Models;

public partial class DataPointModel
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

    public virtual RouteModel RouteModel { get; set; } = null!;
}
