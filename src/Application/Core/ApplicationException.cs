namespace Application.Core;

public class ServiceException(Error error) : Exception(error.Message)
{
    public Error Error { get; } = error;
}