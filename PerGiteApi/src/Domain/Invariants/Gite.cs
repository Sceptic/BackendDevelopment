using Domain.Specs;
using static Domain.Helpers.Helpers;

namespace Domain.Models
{
    public partial class Gite
    {
        private Gite() { }

        public static Gite Create(
            int giteNumber,
            decimal gitePrice,
            bool isAvailable,
            string giteAddress,
            int capacityMin,
            int capacityMax,
            GiteAmenitiesSpec amenities,
            IEnumerable<GiteBedSpec> beds)
        {
            ValidateNumber(giteNumber);
            ValidatePrice(gitePrice);
            ValidateAddress(giteAddress);
            ValidateCapacity(capacityMin, capacityMax);

            var gite = new Gite
            {
                GiteNumber = giteNumber,
                GitePrice = gitePrice,
                IsAvailable = isAvailable,
                GiteAddress = giteAddress,
                CapacityMin = capacityMin,
                CapacityMax = capacityMax,
            };

            gite.Amenities = new GiteAmenities(
                amenities.Wifi, amenities.Bath, amenities.Shower, amenities.HairDryer, amenities.SmallChild,
                amenities.Toiletries, amenities.Desk, amenities.Chair, amenities.Balcony, amenities.Sofa,
                amenities.SofaBed, amenities.MiniFridge, amenities.Kettle, amenities.Cuttlery,
                amenities.EatingArea, amenities.RoomService);

            var bedList = beds?.ToList() ?? throw new ArgumentNullException(nameof(beds));
            Require(bedList.Count > 0, "At least one bed is required.");

            foreach (var b in bedList)
                gite.Beds.Add(new GiteBed(b.Amount1PrBed, b.Amount2PrBed, b.Amount3PrBed, b.BedSort));

            return gite;
        }

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

        public void SetAmenitiesFromSpec(GiteAmenitiesSpec spec)
        {
            Require(spec != null, "Amenities spec must not be null.");

            Amenities = new GiteAmenities(
                spec.Wifi,
                spec.Bath,
                spec.Shower,
                spec.HairDryer,
                spec.SmallChild,
                spec.Toiletries,
                spec.Desk,
                spec.Chair,
                spec.Balcony,
                spec.Sofa,
                spec.SofaBed,
                spec.MiniFridge,
                spec.Kettle,
                spec.Cuttlery,
                spec.EatingArea,
                spec.RoomService);
        }

        public void AddBed(int a1, int a2, int a3, string sort)
        {
            var bed = new GiteBed(a1, a2, a3, sort);
            Beds.Add(bed);
        }

        public void RemoveBed(GiteBed bed)
        {
            Require(bed != null, "Bed must not be null.");
            Require(Beds.Count > 1, "A gite must have at least one bed.");
            Beds.Remove(bed);
        }

        public void ReplaceBeds(IEnumerable<GiteBedSpec> beds)
        {
            var bedList = beds?.ToList() ?? throw new ArgumentNullException(nameof(beds));
            Require(bedList.Count > 0, "At least one bed is required.");

            Beds.Clear();

            foreach (var b in bedList)
                Beds.Add(new GiteBed(b.Amount1PrBed, b.Amount2PrBed, b.Amount3PrBed, b.BedSort));
        }
    }
}
