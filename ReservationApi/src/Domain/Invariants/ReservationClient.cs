using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationClient
{
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new DomainValidationException("FirstName is required.");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new DomainValidationException("LastName is required.");

        if (BirthDate == default)
            throw new DomainValidationException("BirthDate is required.");

        if (BirthDate > DateTime.UtcNow.Date)
            throw new DomainValidationException("BirthDate cannot be in the future.");

        if (FirstName.Length > 50)
            throw new DomainValidationException("FirstName max length is 50.");

        if (LastName.Length > 50)
            throw new DomainValidationException("LastName max length is 50.");
    }
}
