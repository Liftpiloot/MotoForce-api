namespace Logic.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException() : base("Email already exists")
    {
    }
}