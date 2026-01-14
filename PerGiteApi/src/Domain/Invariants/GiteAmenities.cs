using static Domain.Helpers.Helpers;

namespace Domain.Models
{
    public partial class GiteAmenities
    {
        private GiteAmenities() { }

        public GiteAmenities(int giteId)
        {
            GiteId = giteId;
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
