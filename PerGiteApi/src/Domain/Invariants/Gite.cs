using static Domain.Helpers.Helpers;

namespace Domain.Models
{
    public partial class Gite
    {
        private Gite() { }

        //Invariants/checks die alleen bestaan om regels te valideren, doen op zich niks anders.
        private static void ValidateCapacity(int min, int max)
        {
            Require(min > 0, "CapacityMin must be > 0.");
            Require(max > 0, "CapacityMax must be > 0.");
            Require(min <= max, "CapacityMin must be <= CapacityMax.");
        }

        private static void ValidateNumber(int number)
        {
            Require(number > 0, "GiteNumber must be > 0.");
        }

        private static void ValidatePrice(decimal price)
        {
            Require(price >= 0m, "GitePrice must be >= 0.");
            Require(price <= 999.99m, "GitePrice must fit precision(5,2) <= 999.99.");
            Require(decimal.Round(price, 2) == price, "GitePrice must have at most 2 decimals.");
        }

        private static void ValidateAddress(string address)
        {
            Require(!string.IsNullOrWhiteSpace(address), "GiteAddress must be provided.");
            Require(address.Length <= 100, "GiteAddress must be <= 100 characters.");
        }

        // Constructor gebruikt de eerder gedefinieerde checks om ervoor te zorgen dat het object correct geïntialiseerd wordt.
        public Gite(int giteNumber, decimal gitePrice, bool isAvailable, string giteAddress, int capacityMin, int capacityMax)
        {
            ValidateNumber(giteNumber);
            ValidatePrice(gitePrice);
            ValidateAddress(giteAddress);
            ValidateCapacity(capacityMin, capacityMax);

            GiteNumber = giteNumber;
            GitePrice = gitePrice;
            IsAvailable = isAvailable;
            GiteAddress = giteAddress;
            CapacityMin = capacityMin;
            CapacityMax = capacityMax;
        }

        //Mutators, worden gebruikt om waardes te veranderen, nadat zij eerder al zijn geïnitialiseerd, enforcen ook regels.
        public void ChangePrice(decimal newPrice)
        {
            ValidatePrice(newPrice);
            GitePrice = newPrice;
        }

        public void ChangeAddress(string newAddress)
        {
            ValidateAddress(newAddress);
            GiteAddress = newAddress;
        }

        public void ChangeCapacity(int min, int max)
        {
            ValidateCapacity(min, max);

            CapacityMin = min;
            CapacityMax = max;
        }

        //Zet gites tijdelijk buiten werking voor rennovaties of andere evenementen.
        public void MarkAvailable() => IsAvailable = true;
        public void MarkUnavailable() => IsAvailable = false;

        public void SetAmenities(GiteAmenities amenities)
        {
            Require(amenities != null, "Amenities must not be null."); // Een gite MOET altijd amenities hebben.
            Require(amenities.GiteId == 0 || amenities.GiteId == GiteId, "Amenities must belong to this Gite.");
            Amenities = amenities;
        }

        public void AddBed(GiteBed bed)
        {
            Require(bed != null, "Bed must not be null.");
            Require(bed.GiteId == 0 || bed.GiteId == GiteId, "Bed must belong to this Gite.");
            Beds.Add(bed);
        }

        public void RemoveBed(GiteBed bed)
        {
            Require(bed != null, "Bed must not be null.");
            Beds.Remove(bed);
        }
    }
}
