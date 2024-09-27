using Interface.Models;

namespace Logic.Classes;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public virtual List<Route> Routes { get; set; } = new List<Route>();
    
    public User(UserModel userModel)
    {
        Id = userModel.Id;
        Name = userModel.Name;
        Email = userModel.Email;
        Password = userModel.Password;
        ProfilePic = userModel.ProfilePic;
        Routes = userModel.Routes.Select(r => new Route(r)).ToList();
    }
}