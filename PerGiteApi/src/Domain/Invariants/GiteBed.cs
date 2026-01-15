using static Domain.Helpers.Helpers;

namespace Domain.Models
{
    public partial class GiteBed
    {
        private GiteBed() { }

        internal GiteBed(int a1, int a2, int a3, string sort)
        {
            ValidateAmounts(a1, a2, a3);
            ValidateSort(sort);

            Amount1PrBed = a1;
            Amount2PrBed = a2;
            Amount3PrBed = a3;
            BedSort = sort;
        }

        private static void ValidateAmounts(int a1, int a2, int a3)
        {
            Require(a1 >= 0, "Amount1PrBed must be >= 0.");
            Require(a2 >= 0, "Amount2PrBed must be >= 0.");
            Require(a3 >= 0, "Amount3PrBed must be >= 0.");
        }

        private static void ValidateSort(string sort)
        {
            Require(!string.IsNullOrWhiteSpace(sort), "BedSort must be provided.");
        }

        public void ChangeAmounts(int amount1PrBed, int amount2PrBed, int amount3PrBed)
        {
            ValidateAmounts(amount1PrBed, amount2PrBed, amount3PrBed);
            Amount1PrBed = amount1PrBed;
            Amount2PrBed = amount2PrBed;
            Amount3PrBed = amount3PrBed;
        }

        public void ChangeSort(string bedSort)
        {
            ValidateSort(bedSort);
            BedSort = bedSort;
        }
    }
}
