using Interface.Factories;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using IRouteHandler = Interface.Interface.Handlers.IRouteHandler;

namespace MotoForce_api.Controllers;

[Route("Route")]
[ApiController]
public class RouteController(ILogicFactoryBuilder logicFactoryBuilder) : Controller
{
    private readonly IRouteHandler _routeHandler =
        logicFactoryBuilder.BuildHandlerFactory().GetRouteHandler();
    private readonly IUserHandler _userHandler =
        logicFactoryBuilder.BuildHandlerFactory().GetUserHandler();
    
    
    [HttpPost]
    [Route("postRoute")]
    public async Task<IActionResult> PostRoute(int userId)
    {
        try
        {
            int routeId = await _routeHandler.CreateRoute(userId);
            return Ok(new{RouteId = routeId});
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetRoute(int routeId)
    {
        try
        {
            RouteModel route = await _routeHandler.GetRoute(routeId);
            if (route.Distance < 0.01 && route.DataPoints.Count >= 3)
            {
                await _routeHandler.CalculateRouteStats(routeId);
            }
            return Ok(new {Route = await _routeHandler.GetRoute(routeId)});
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpDelete]
    [Route("")]
    public async Task<IActionResult> DeleteRoute(int routeId)
    {
        try
        {
            await _routeHandler.DeleteRoute(routeId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpPost]
    [Route("postDataPoints")]
    public async Task<IActionResult> PostDataPoint(DataPointModel[] dataPoints)
    {
        try
        {
            await _routeHandler.CreateDataPoints(dataPoints);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpGet]
    [Route("MaxSpeed")]
    public async Task<IActionResult> GetMaxSpeed(int routeId)
    {
        try
        {
            var maxSpeed = await _routeHandler.GetMaxSpeed(routeId);
            var route = await _routeHandler.GetRoute(routeId);
            var highScore = await _userHandler.GetMaxSpeed(route.UserId);
            
            return Ok(new {MaxSpeed = maxSpeed, HighScore = highScore});
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpGet]
    [Route("Maxlean")]
    public async Task<IActionResult> GetMaxLean(int routeId)
    {
        try
        {
            var maxLean = await _routeHandler.GetMaxLean(routeId);
            var route = await _routeHandler.GetRoute(routeId);
            var highScore = await _userHandler.GetMaxLean(route.UserId);
            
            return Ok(new {MaxLean = maxLean, HighScore = highScore});
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
    
    [HttpGet]
    [Route("MaxG")]
    public async Task<IActionResult> GetMaxG(int routeId)
    {
        try
        {
            var maxG = await _routeHandler.GetMaxG(routeId);
            var route = await _routeHandler.GetRoute(routeId);
            var highScore = await _userHandler.GetMaxG(route.UserId);

            return Ok(new { MaxG = maxG, HighScore = highScore });
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
}