namespace EmployeeDirectory.Application.Exceptions;

public class InvalidFieldException : Exception
{
    public InvalidFieldException(string message) : base(message)
    {
        
    }
}

