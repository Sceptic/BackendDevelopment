using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class Vehicle
{
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(RegistrationPlate))
            throw new DomainValidationException("RegistrationPlate is required.");

        if (RegistrationPlate.Length > 50)
            throw new DomainValidationException("RegistrationPlate max length is 50.");
    }
}
