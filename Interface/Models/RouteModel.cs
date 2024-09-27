namespace Interface.Models;

public partial class RouteModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Distance { get; set; }

    public virtual ICollection<DataPointModel> DataPoints { get; set; } = new List<DataPointModel>();

    public virtual UserModel UserModel { get; set; } = null!;
}
