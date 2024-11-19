using Interface.Interface.Dal;
using Interface.Models;
using Logic.Handlers;
using Moq;

namespace MotoForce_Tests.Handlers;

[TestClass]
public class RouteHandlerTests
{
    private Mock<IRouteDal> _routeDal;
    private RouteHandler _routeHandler;
    
    [TestInitialize]
    public void Initialize()
    {
        _routeDal = new Mock<IRouteDal>();
        _routeHandler = new RouteHandler(_routeDal.Object);
    }
    
    [TestMethod]
    public async Task CreateRoute_ShouldReturnRouteId_WhenRouteIsCreated()
    {
        // Arrange
        _routeDal.Setup(x => x.CreateRoute(It.IsAny<int>())).ReturnsAsync(1);
        
        // Act
        var result = await _routeHandler.CreateRoute(1);
        
        // Assert
        Assert.AreEqual(1, result);
    }
    
    
    [TestMethod]
    public async Task CreateDataPoints_ShouldReturnVoid_WhenDataPointsAreCreated()
    {
        // Arrange
        var dataPoints = new DataPointModel[]
        {
            new DataPointModel
            {
                RouteId = 1,
                Lat= 1.0,
                Lon = 1.0,
                Speed = 1.0,
                Lean = 1.0,
                LateralG = 1.0
            }
        };
        
        // Act
        await _routeHandler.CreateDataPoints(dataPoints);
        
        // Assert
        _routeDal.Verify(x => x.CreateDataPoint(dataPoints), Times.Once);
    }
    
    
    [TestMethod]
    public async Task GetMaxSpeed_ShouldReturnMaxSpeed_WhenRouteExists()
    {
        // Arrange
        _routeDal.Setup(x => x.GetMaxSpeed(It.IsAny<int>())).ReturnsAsync(1.0);
        
        // Act
        var result = await _routeHandler.GetMaxSpeed(1);
        
        // Assert
        Assert.AreEqual(1.0, result);
    }
    
    [TestMethod]
    public async Task GetMaxLean_ShouldReturnMaxLean_WhenRouteExists()
    {
        // Arrange
        _routeDal.Setup(x => x.GetMaxLean(It.IsAny<int>())).ReturnsAsync(1.0);
        
        // Act
        var result = await _routeHandler.GetMaxLean(1);
        
        // Assert
        Assert.AreEqual(1.0, result);
    }
    
    [TestMethod]
    public async Task GetMaxG_ShouldReturnMaxG_WhenRouteExists()
    {
        // Arrange
        _routeDal.Setup(x => x.GetMaxG(It.IsAny<int>())).ReturnsAsync(1.0);
        
        // Act
        var result = await _routeHandler.GetMaxG(1);
        
        // Assert
        Assert.AreEqual(1.0, result);
    }
    
    [TestMethod]
    public async Task GetRoute_ShouldReturnRouteModel_WhenRouteExists()
    {
        // Arrange
        var routeModel = new RouteModel
        {
            Id = 1,
            UserId = 1,
            DataPoints = new DataPointModel[]
            {
                new DataPointModel
                {
                    RouteId = 1,
                    Lat= 1.0,
                    Lon = 1.0,
                    Speed = 1.0,
                    Lean = 1.0,
                    LateralG = 1.0
                }
            }
        };
        
        _routeDal.Setup(x => x.GetRoute(It.IsAny<int>())).ReturnsAsync(routeModel);
        
        // Act
        var result = await _routeHandler.GetRoute(1);
        
        // Assert
        Assert.AreEqual(routeModel, result);
    }
    
    [TestMethod]
    public async Task DeleteRoute_ShouldReturnVoid_WhenRouteIsDeleted()
    {
        // Arrange
        _routeDal.Setup(x => x.DeleteRoute(It.IsAny<int>()));
        
        // Act
        await _routeHandler.DeleteRoute(1);
        
        // Assert
        _routeDal.Verify(x => x.DeleteRoute(1), Times.Once);
    }
    
}