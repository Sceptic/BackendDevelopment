using static Domain.Helpers.Helpers;

namespace Domain.Models
{
    public partial class GiteAmenities
    {
        private GiteAmenities() { }

        internal GiteAmenities(
            bool? wifi, bool? bath, bool? shower, bool? hairDryer, bool? smallChild,
            bool? toiletries, bool? desk, bool? chair, bool? balcony, bool? sofa,
            bool? sofaBed, bool? miniFridge, bool? kettle, bool? cuttlery,
            bool? eatingArea, bool? roomService)
        {
            Wifi = wifi;
            Bath = bath;
            Shower = shower;
            HairDryer = hairDryer;
            SmallChild = smallChild;
            Toiletries = toiletries;
            Desk = desk;
            Chair = chair;
            Balcony = balcony;
            Sofa = sofa;
            SofaBed = sofaBed;
            MiniFridge = miniFridge;
            Kettle = kettle;
            Cuttlery = cuttlery;
            EatingArea = eatingArea;
            RoomService = roomService;
        }

        public void SetWifi(bool? value) => Wifi = value;
        public void SetBath(bool? value) => Bath = value;
        public void SetShower(bool? value) => Shower = value;
        public void SetHairDryer(bool? value) => HairDryer = value;
        public void SetSmallChild(bool? value) => SmallChild = value;
        public void SetToiletries(bool? value) => Toiletries = value;
        public void SetDesk(bool? value) => Desk = value;
        public void SetChair(bool? value) => Chair = value;
        public void SetBalcony(bool? value) => Balcony = value;
        public void SetSofa(bool? value) => Sofa = value;
        public void SetSofaBed(bool? value) => SofaBed = value;
        public void SetMiniFridge(bool? value) => MiniFridge = value;
        public void SetKettle(bool? value) => Kettle = value;
        public void SetCuttlery(bool? value) => Cuttlery = value;
        public void SetEatingArea(bool? value) => EatingArea = value;
        public void SetRoomService(bool? value) => RoomService = value;
    }
}
