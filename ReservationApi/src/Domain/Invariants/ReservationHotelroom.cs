using Domain.ErrorHandling;

namespace Domain.Models;

public sealed partial class ReservationHotelroom
{
    public void EnsureValid()
    {
        if (RoomId <= 0)
            throw new DomainValidationException("RoomId must be > 0.");

        if (HotelroomDiscount < 0m || HotelroomDiscount > 1m)
            throw new DomainValidationException("HotelroomDiscount must be between 0 and 1 (fractional).");
    }
}
