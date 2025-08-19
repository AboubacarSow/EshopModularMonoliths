namespace Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) 
        : base(message)
    {
    }

    public NotFoundException(string name, object key) 
        : base($"Ressource \"{name}\" ({key}) was not found.")
    {
    }
}
public class InternalServerException: Exception
{
    public string? Details { get; }
    
    public InternalServerException(string message)
        : base(message)
    {
    }

    public InternalServerException(string message, string details) 
        : base(message)
    {
        Details = details;
    }

}