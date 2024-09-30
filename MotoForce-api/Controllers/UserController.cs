using FluentValidation;
using Interface.Factories;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Classes;
using Logic.Exceptions;
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
    
    // register
    [HttpPost]  
    [Route("/Register")]
    public IActionResult Register(UserModel userModel)
    {
        try
        {
            _userHandler.Register(userModel);
            return Ok();
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNameAlreadyExistsException)
        {
            return Conflict(new { message = "Username already exists" });
        }
        catch (EmailAlreadyExistsException)
        {
            return Conflict(new { message = "Email already exists" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
}