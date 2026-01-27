using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationFacility
{
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(Facility))
            throw new DomainValidationException("Facility is required.");

        if (Facility.Length > 50)
            throw new DomainValidationException("Facility max length is 50.");

        if (FacilityDiscount < 0m || FacilityDiscount > 1m)
            throw new DomainValidationException("FacilityDiscount must be between 0 and 1 (fractional).");
    }
}
