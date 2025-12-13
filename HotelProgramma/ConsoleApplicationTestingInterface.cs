using HotelProgramma.Data;
using HotelProgramma.Models;

public class ConsoleApplicationTestingInterface
{
    public ConsoleApplicationTestingInterface()
    {
        using IUnitOfWork uow = new UnitOfWork();

        var update = new HotelRoom
        {
            RoomNumber = 21,                 // REQUIRED identity
            HotelroomPrice = 145.43m,         // change price
            IsAvailable = false,              // change availability

            Bed = new HotelRoomBed
            {
                Amount2PrBed = 1              // change only this
                                              // other bed fields omitted → unchanged
            },

            Amenities = new HotelRoomAmenities
            {
                Wifi = true,
                Roomservice = false
                // rest omitted → unchanged
            }
        };

        uow.Hotels.Post(update);

        uow.Complete();
    }
}
