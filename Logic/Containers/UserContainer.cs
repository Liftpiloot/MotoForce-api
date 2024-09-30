using Interface.Interface.Dal;
using Interface.Models;
using Logic.Exceptions;
using Logic.Validators;

namespace Logic.Containers;

public class UserContainer
{
    private readonly IUserDal userDal;
    
    public UserContainer(IUserDal userDal)
    {
        this.userDal = userDal;
    }
    
    public void Register(UserModel userModel)
    {
        // check if model is valid using fluent validation
        UserValidator validator = new UserValidator();
        validator.ValidateUser(userModel);
        
        // check if username already exists
        if (userDal.UserNameExists(userModel.Name))
        {
            throw new UserNameAlreadyExistsException();
        }
        
        // check if email already exists
        if (userDal.EmailExists(userModel.Email))
        {
            throw new EmailAlreadyExistsException();
        }
        
        // Hash password
        userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
        
        // Register user
        userDal.Register(userModel);
    }
}