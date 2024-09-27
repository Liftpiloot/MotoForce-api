using Interface.Factories;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Classes;
using Microsoft.AspNetCore.Mvc;

namespace MotoForce_api.Controllers;

[ApiController]
public class UserController(ILogicFactoryBuilder logicFactoryBuilder) : Controller
{
    private readonly IUserHandler _userHandler =
        logicFactoryBuilder.BuildHandlerFactory().GetUserHandler();
    
    // get all users
    [HttpGet]
    [Route("/Get")]
    public IActionResult GetUsers()
    {
        try
        {
            return Ok(_userHandler.GetUsers());
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }
}