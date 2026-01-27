using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationCamping
{
    public void EnsureValid()
    {
        if (CampingId <= 0)
            throw new DomainValidationException("CampingId must be > 0.");

        if (CampingDiscount < 0m || CampingDiscount > 1m)
            throw new DomainValidationException("CampingDiscount must be between 0 and 1 (fractional).");
    }
}
