using HotelProgramma.Data;
using HotelProgramma.Models;

public class ConsoleApplicationTestingInterface
{
    public ConsoleApplicationTestingInterface()
    {
        using IUnitOfWork uow = new UnitOfWork();

        uow.Reservations.Add(

        uow.Complete();
    }
}
