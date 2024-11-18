using Interface.Factories;
using Interface.Models;
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
            return Ok(new {Route = await _routeHandler.GetRoute(routeId)});
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
            return Ok(new {MaxSpeed = await _routeHandler.GetMaxSpeed(routeId)});
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
            return Ok(new {MaxLean = await _routeHandler.GetMaxLean(routeId)});
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
            return Ok(new {MaxG = await _routeHandler.GetMaxG(routeId)});
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
}