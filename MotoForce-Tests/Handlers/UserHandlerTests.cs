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
    
    [TestMethod]
    public async Task MaxSpeed_ShouldReturnMaxSpeed_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var maxSpeed = 100.0;

        _userDal.Setup(x => x.GetMaxSpeed(It.IsAny<int>())).ReturnsAsync(maxSpeed);

        // Act
        var result = await _userHandler.GetMaxSpeed(userId);

        // Assert
        Assert.AreEqual(maxSpeed, result);
    }
    
    [TestMethod]
    public async Task MaxLean_ShouldReturnMaxLean_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var maxLean = 100.0;

        _userDal.Setup(x => x.GetMaxLean(It.IsAny<int>())).ReturnsAsync(maxLean);

        // Act
        var result = await _userHandler.GetMaxLean(userId);

        // Assert
        Assert.AreEqual(maxLean, result);
    }
    
    [TestMethod]
    public async Task MaxG_ShouldReturnMaxG_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var maxG = 100.0;

        _userDal.Setup(x => x.GetMaxG(It.IsAny<int>())).ReturnsAsync(maxG);

        // Act
        var result = await _userHandler.GetMaxG(userId);

        // Assert
        Assert.AreEqual(maxG, result);
    }

    [TestMethod]
    public async Task GetFriends_ShouldReturnFriends_WhenFriendsExist()
    {
        // Arrange
        var userId = 1;
        var friends = new List<UserModel>
        {
            new UserModel
            {
                Id = 2,
                Name = "Jane Doe"
            }
        };

        _userDal.Setup(x => x.GetFriends(It.IsAny<int>())).ReturnsAsync(friends);

        // Act
        var result = await _userHandler.GetFriends(userId);

        // Assert
        Assert.AreEqual(friends, result);
    }

    [TestMethod]
    public async Task GetFriends_ShouldReturnEmptyList_WhenFriendsDoNotExist()
    {
        // Arrange
        var userId = 1;
        var friends = new List<UserModel>();

        _userDal.Setup(x => x.GetFriends(It.IsAny<int>())).ReturnsAsync(friends);

        // Act
        var result = await _userHandler.GetFriends(userId);

        // Assert
        Assert.AreEqual(friends, result);
    }

    [TestMethod]
    public async Task AddFriend_ShouldCallDal_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var email = "TestFriend@test.nl";
        
        // Act
        await _userHandler.AddFriend(userId, email);

        // Assert
        _userDal.Verify(x => x.AddFriend(userId, email), Times.Once);
    }
    
}