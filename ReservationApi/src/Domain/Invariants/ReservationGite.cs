using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationGite
{
    public void EnsureValid()
    {
        if (GiteId <= 0)
            throw new DomainValidationException("GiteId must be > 0.");

        if (GiteDiscount < 0m || GiteDiscount > 1m)
            throw new DomainValidationException("GiteDiscount must be between 0 and 1 (fractional).");
    }
}
