using System.Net.Sockets;
using FluentValidation;
using Interface.Factories;
using Interface.Interface.Handlers;
using Interface.Models;
using Logic.Classes;
using Logic.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MotoForce_api.Hubs;

namespace MotoForce_api.Controllers;

[ApiController]
public class UserController(ILogicFactoryBuilder logicFactoryBuilder, IHubContext<NotificationHub> hubContext) : Controller
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
    
    // login
    [HttpGet]
    [Route("/Login")]
    public async Task<IActionResult> Login(string identifier, string password)
    {
        if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
        {
            return BadRequest("Identifier or password is empty");
        }
        try
        {
            UserModel? user = await _userHandler.Login(identifier);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Ok(user);
            }
            return NotFound("User not found or password incorrect");
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    
    // get most recent route
    [HttpGet]
    [Route("/GetRoutes")]
    public async Task<IActionResult> GetRoutes(int userId, int count)
    {
        try
        {
            List<RouteModel> routes = await _userHandler.GetRoutes(userId, count);
            return Ok(routes);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // get friends
    [HttpGet]
    [Route("/GetFriends")]
    public async Task<IActionResult> GetFriends(int userId)
    {
        try
        {
            List<UserModel> friends = await _userHandler.GetFriends(userId);
            return Ok(friends);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // add friend
    [HttpPost]
    [Route("/AddFriend")]
    public async Task<IActionResult> AddFriend(int userId, string email)
    {
        try
        {
            UserModel user = await _userHandler.GetUser(userId);
            if (user.Email == email)
            {
                return Conflict(new { message = "Cannot add self as friend" });
            }
            // check if not already friends
            List<UserModel> friends = await _userHandler.GetFriends(userId);
            List<FriendModel> friendRequests = await _userHandler.GetFriendRequests(userId);
            foreach (var friend in friends)
            {
                if (friend.Email == email)
                {
                    return Conflict(new { message = "Already friends" });
                }
            }
            foreach (var friendRequest in friendRequests)
            {
                UserModel friend = await _userHandler.GetUser(friendRequest.SenderId);
                if (friend.Email == email)
                {
                    return Conflict(new { message = "Friend request already sent" });
                }
            }
            
            await _userHandler.AddFriend(userId, email);
            UserModel friendUser = await _userHandler.getUserByEmail(email);
            await hubContext.Clients.Group(friendUser.Id.ToString()).SendAsync("FriendRequest", "You received a friend request from " + user.Name);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // remove friend
    [HttpDelete]
    [Route("/RemoveFriend")]
    public async Task<IActionResult> RemoveFriend(int userId, int friendId)
    {
        try
        {
            List<UserModel> friends = await _userHandler.GetFriends(userId);
            bool found = false;
            foreach (var friend in friends)
            {
                if (friend.Id == friendId)
                {
                    await _userHandler.RemoveFriend(userId, friendId);
                    return Ok();
                }
            }
            return NotFound(new { message = "Friend not found" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // accept friend
    [HttpPost]
    [Route("/AcceptFriend")]
    public async Task<IActionResult> AcceptFriend(int friendId)
    {
        try
        {
            await _userHandler.AcceptFriend(friendId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // deny friend
    [HttpDelete]
    [Route("/DenyFriend")]
    public async Task<IActionResult> DenyFriend(int friendId)
    {
        try
        {
            await _userHandler.DenyFriend(friendId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    
    // get friend requests
    [HttpGet]
    [Route("/GetFriendRequests")]
    public async Task<IActionResult> GetFriendRequests(int userId)
    {
        try
        {
            List<FriendModel> friendRequests = await _userHandler.GetFriendRequests(userId);
            List<FriendRequestResponse> friendRequestsNameAndId = new List<FriendRequestResponse>();
            // get the senderid name
            foreach (var friendRequest in friendRequests)
            {
                UserModel user = await _userHandler.GetUser(friendRequest.SenderId);
                friendRequestsNameAndId.Add(new FriendRequestResponse
                {
                    Id = friendRequest.Id,
                    Name = user.Name
                });
            }
            return Ok(friendRequestsNameAndId);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
}