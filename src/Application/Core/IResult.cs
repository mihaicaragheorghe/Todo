namespace Application.Core;

public interface IResult
{
    Error? Error { get; }
    bool IsSuccessful { get; }
}
