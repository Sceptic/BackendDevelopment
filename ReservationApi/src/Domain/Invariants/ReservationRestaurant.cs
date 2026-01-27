using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationRestaurant
{
    public void EnsureValidWithin(Reservation reservation)
    {
        EnsureValid();

        if (TableReservationStart < reservation.ReservationStart ||
            TableReservationEnd > reservation.ReservationEnd)
        {
            throw new DomainValidationException("Table reservation period must fall within the reservation period.");
        }
    }

    public void EnsureValid()
    {
        if (TableId <= 0)
            throw new DomainValidationException("TableId must be > 0.");

        if (TableReservationStart == default)
            throw new DomainValidationException("TableReservationStart is required.");

        if (TableReservationEnd == default)
            throw new DomainValidationException("TableReservationEnd is required.");

        if (TableReservationEnd <= TableReservationStart)
            throw new DomainValidationException("TableReservationEnd must be after TableReservationStart.");

        if (TableBill < 0m)
            throw new DomainValidationException("TableBill cannot be negative.");

        if (TableDiscount < 0m || TableDiscount > 1m)
            throw new DomainValidationException("TableDiscount must be between 0 and 1 (fractional).");
    }
}
