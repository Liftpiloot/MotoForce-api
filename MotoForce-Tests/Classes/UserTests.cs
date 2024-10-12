using Interface.Models;
using Logic.Classes;

namespace MotoForce_Tests.Classes;

[TestClass]
public class UserTests
{
    private UserModel _userModel = null!;
    private User _User = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userModel = new UserModel
        {
            Id = 1,
            Name = "John Doe",
            Email = "JohnDoe@email.nl",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            ProfilePic = "https://example.com/profile.jpg",
            Routes = new List<RouteModel>
            {
                new RouteModel
                {
                    Id = 1,
                    UserId = 1,
                    Distance = 100,
                    DataPoints = new List<DataPointModel>
                    {
                        new DataPointModel
                        {
                            Id = 1,
                            RouteId = 1,
                            Lat = 1.0,
                            Lon = 1.0,
                            Lean = 1.0,
                            LateralG = 1.0,
                            Acceleration = 1.0,
                            Timestamp = DateTime.Now,
                            Speed = 1.0
                        }
                    }
                }
            }
        };
        _User = new User(_userModel);
    }
}