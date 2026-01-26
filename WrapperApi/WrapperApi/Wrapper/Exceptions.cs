namespace WrapperApi.Wrapper;

public sealed class ReservationValidationException : Exception
{
    public ReservationValidationException(string message) : base(message) { }
}

public sealed class ReservationConflictException : Exception
{
    public ReservationConflictException(string message) : base(message) { }
}
