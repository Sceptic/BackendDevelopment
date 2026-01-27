using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class Reservation
{
    public void EnsureValid()
    {
        EnsureIdInvariants();
        EnsureStatusInvariants();
        EnsureMoneyInvariants();
        EnsureDateInvariants();
        EnsureChildInvariants();
    }

    public void SetPeriod(DateTime start, DateTime end)
    {
        if (start == default)
            throw new DomainValidationException("ReservationStart is required.");

        if (end == default)
            throw new DomainValidationException("ReservationEnd is required.");

        if (end <= start)
            throw new DomainValidationException("ReservationEnd must be after ReservationStart.");

        ReservationStart = start;
        ReservationEnd = end;
    }

    public void SetStatuses(string reservationStatus, string paymentStatus)
    {
        if (string.IsNullOrWhiteSpace(reservationStatus))
            throw new DomainValidationException("ReservationStatus is required.");

        if (string.IsNullOrWhiteSpace(paymentStatus))
            throw new DomainValidationException("PaymentStatus is required.");

        ReservationStatus = reservationStatus.Trim();
        PaymentStatus = paymentStatus.Trim();
    }

    public void SetPricing(decimal reservationPrice, decimal discount, decimal touristTarif)
    {
        if (reservationPrice < 0)
            throw new DomainValidationException("ReservationPrice cannot be negative.");

        EnsurePercentLike(discount, "Discount");
        EnsurePercentLike(touristTarif, "TouristTarif");

        ReservationPrice = reservationPrice;
        Discount = discount;
        TouristTarif = touristTarif;
    }

    private void EnsureIdInvariants()
    {
        if (AccountId <= 0)
            throw new DomainValidationException("AccountId must be > 0.");
    }

    private void EnsureStatusInvariants()
    {
        if (string.IsNullOrWhiteSpace(ReservationStatus))
            throw new DomainValidationException("ReservationStatus is required.");

        if (string.IsNullOrWhiteSpace(PaymentStatus))
            throw new DomainValidationException("PaymentStatus is required.");
    }

    private void EnsureMoneyInvariants()
    {
        if (ReservationPrice < 0)
            throw new DomainValidationException("ReservationPrice cannot be negative.");

        EnsurePercentLike(Discount, "Discount");
        EnsurePercentLike(TouristTarif, "TouristTarif");
    }

    private void EnsureDateInvariants()
    {
        if (ReservationStart == default)
            throw new DomainValidationException("ReservationStart is required.");

        if (ReservationEnd == default)
            throw new DomainValidationException("ReservationEnd is required.");

        if (ReservationEnd <= ReservationStart)
            throw new DomainValidationException("ReservationEnd must be after ReservationStart.");
    }

    private void EnsureChildInvariants()
    {
        foreach (var c in Clients) c.EnsureValid();
        foreach (var g in Gites) g.EnsureValid();
        foreach (var h in Hotelrooms) h.EnsureValid();
        foreach (var c in Campings) c.EnsureValid();
        foreach (var f in Facilities) f.EnsureValid();
        foreach (var v in Vehicles) v.EnsureValid();
        foreach (var r in Restaurants) r.EnsureValidWithin(this);
    }

    private static void EnsurePercentLike(decimal value, string name)
    {
        if (value < 0m || value > 1m)
            throw new DomainValidationException($"{name} must be between 0 and 1 (fractional).");
    }
}
