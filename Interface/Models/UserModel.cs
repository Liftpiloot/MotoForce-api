namespace Interface.Models;

public partial class UserModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public virtual ICollection<RouteModel> Routes { get; set; } = new List<RouteModel>();
}
