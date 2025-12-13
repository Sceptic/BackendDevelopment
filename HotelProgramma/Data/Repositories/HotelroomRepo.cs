using HotelProgramma.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelProgramma.Data.Repositories
{
    internal class HotelroomRepo : IHotelroomRepo
    {
        private readonly DbMarconnes _db;

        public HotelroomRepo(DbMarconnes db)
        {
            _db = db;
        }

        public HotelRoom Get(int id)
        {
            return _db.tblHotelRoom
                .Include(x => x.Bed)
                .Include(x => x.Amenities)
                .FirstOrDefault(x => x.RoomNumber == id);
        }

        public void Post(HotelRoom hotelroom)
        {
            _db.tblHotelRoom.Add(hotelroom);
        }

        public void Update(HotelRoom updated)
        {
            var existing = _db.tblHotelRoom
                .Include(x => x.Bed)
                .Include(x => x.Amenities)
                .First(x => x.RoomNumber == updated.RoomNumber);

            existing.HotelroomPrice = updated.HotelroomPrice ?? existing.HotelroomPrice;
            existing.IsAvailable = updated.IsAvailable ?? existing.IsAvailable;
            existing.CapacityMin = updated.CapacityMin ?? existing.CapacityMin;
            existing.CapacityMax = updated.CapacityMax ?? existing.CapacityMax;

            if (updated.Bed != null)
            {
                existing.Bed.Amount1PrBed = updated.Bed.Amount1PrBed ?? existing.Bed.Amount1PrBed;
                existing.Bed.Amount2PrBed = updated.Bed.Amount2PrBed ?? existing.Bed.Amount2PrBed;
                existing.Bed.Amount3PrBed = updated.Bed.Amount3PrBed ?? existing.Bed.Amount3PrBed;
                existing.Bed.BedSort = updated.Bed.BedSort ?? existing.Bed.BedSort;
            }

            if (updated.Amenities != null)
            {
                var a = updated.Amenities;
                var e = existing.Amenities;

                e.Wifi = a.Wifi ?? e.Wifi;
                e.Bath = a.Bath ?? e.Bath;
                e.Shower = a.Shower ?? e.Shower;
                e.Hairdryer = a.Hairdryer ?? e.Hairdryer;
                e.Smallchild = a.Smallchild ?? e.Smallchild;
                e.Toiletries = a.Toiletries ?? e.Toiletries;
                e.Desk = a.Desk ?? e.Desk;
                e.Chair = a.Chair ?? e.Chair;
                e.Balcony = a.Balcony ?? e.Balcony;
                e.Sofa = a.Sofa ?? e.Sofa;
                e.Sofabed = a.Sofabed ?? e.Sofabed;
                e.Minifridge = a.Minifridge ?? e.Minifridge;
                e.Kettle = a.Kettle ?? e.Kettle;
                e.Cuttlery = a.Cuttlery ?? e.Cuttlery;
                e.Eatingarea = a.Eatingarea ?? e.Eatingarea;
                e.Roomservice = a.Roomservice ?? e.Roomservice;
            }
        }

        public void Delete(int roomNumber)
        {
            var hotelroom = new HotelRoom { RoomNumber = roomNumber };
            _db.tblHotelRoom.Attach(hotelroom);
            _db.tblHotelRoom.Remove(hotelroom);
        }
    }

    public interface IHotelroomRepo
    {
        HotelRoom Get(int id);
        void Post(HotelRoom hotelroom);
        void Update(HotelRoom hotelroom);
        void Delete(int id);
    }
}
