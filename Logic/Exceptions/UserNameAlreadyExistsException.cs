namespace Logic.Exceptions;

public class UserNameAlreadyExistsException : Exception
{
    public UserNameAlreadyExistsException() : base("Username already exists")
    {
    }
}