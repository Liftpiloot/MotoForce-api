using Interface.Interface.Dal;
using Interface.Models;
using Logic.Handlers;
using Moq;

namespace MotoForce_Tests.Handlers;

[TestClass]
public class UserHandlerTests
{
    private Mock<IUserDal> _userDal;
    private UserHandler _userHandler;

    [TestInitialize]
    public void Initialize()
    {
        _userDal = new Mock<IUserDal>();
        _userHandler = new UserHandler(_userDal.Object);
    }
    
    // Login tests
    [TestMethod]
    public async Task Login_ShouldReturnUserModel_WhenUserExists()
    {
        // Arrange
        var userModel = new UserModel
        {
            Id = 1,
            Name = "John Doe",
            Email = "JohnDoe@email.nl",
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };

        _userDal.Setup(x => x.Login(It.IsAny<string>())).ReturnsAsync(userModel);

        // Act
        var result = await _userHandler.Login(userModel.Name);

        // Assert
        Assert.AreEqual(userModel, result);
    }
    
    [TestMethod]
    public async Task Login_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        _userDal.Setup(x => x.Login(It.IsAny<string>())).ReturnsAsync((UserModel?)null);

        // Act
        var result = await _userHandler.Login("John Doe");

        // Assert
        Assert.IsNull(result);
    }

}