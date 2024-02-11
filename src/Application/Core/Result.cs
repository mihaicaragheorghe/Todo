namespace Application.Core;

public class Result<TValue> : IResult
{
    public TValue? Value { get; } = default;
    public Error? Error { get; }
    public bool IsSuccessful { get; }

    private Result(TValue? value)
    {
        Value = value;
        IsSuccessful = true;
    }

    private Result(Error error)
    {
        Error = error;
        IsSuccessful = false;
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure(error);

    public TNextValue Match<TNextValue>(
        Func<TValue, TNextValue> onSuccess, 
        Func<Error, TNextValue> onFailure) => 
            IsSuccessful ? onSuccess(Value!) : onFailure((Error)Error!);
}