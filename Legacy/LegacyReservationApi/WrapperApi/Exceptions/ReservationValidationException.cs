namespace WrapperApi.Exceptions;

public sealed class ReservationValidationException : Exception
{
    public ReservationValidationException(string message) : base(message) { }
}