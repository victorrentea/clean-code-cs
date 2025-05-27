using System.Globalization;

namespace Victor.Training.Cleancode.VideoStore
{
    public enum PriceCode
    {
        Regular,
        NewRelease,
        Children
    }

    public record Movie(string Title, PriceCode PriceCode);

    public record Rental(Movie Movie, int DaysRented)
    {
        private bool EligibleForBonus => Movie.PriceCode == PriceCode.NewRelease && DaysRented >= 2;

        public int FrequentRenterPoints => EligibleForBonus ? 2 : 1;

        public decimal DetermineRentalAmount()
        {
            return Movie.PriceCode switch
            {
                PriceCode.Regular => DetermineRegularAmount(),
                PriceCode.NewRelease => DetermineNewReleaseAmount(),
                PriceCode.Children => DetermineChildrenAmount(),
                _ => throw new ArgumentOutOfRangeException(nameof(Movie.PriceCode)),
            };
        }

        private decimal DetermineChildrenAmount()
        {
            const decimal BasePrice = 1.5m;
            const int ExtraDaysThreshold = 3;
            const decimal ExtraDailyRate = 1.5m;
            
            decimal rentalAmount = BasePrice;
            if (DaysRented > ExtraDaysThreshold)
            {
                rentalAmount += (DaysRented - ExtraDaysThreshold) * ExtraDailyRate;
            }

            return rentalAmount;
        }

        private decimal DetermineNewReleaseAmount()
        {
            const int NewReleaseDailyRate = 3;
            return DaysRented * NewReleaseDailyRate;
        }

        private decimal DetermineRegularAmount()
        {
            const decimal RegularPriceBase = 2;
            const decimal RegularPriceExtraDailyRate = 1.5m;
            const int RegularPriceExtraDaysThreshold = 2;

            decimal rentalAmount = RegularPriceBase;
            if (DaysRented > RegularPriceExtraDaysThreshold)
            {
                rentalAmount += (DaysRented - RegularPriceExtraDaysThreshold) * RegularPriceExtraDailyRate;
            }

            return rentalAmount;
        }
    }

    public record Customer(string Name)
    {
        private readonly List<Rental> _rentals = new();

        public void AddRental(Movie movie, int daysRented)
        {
            _rentals.Add(new Rental(movie, daysRented));
        }

        public string PrintStatement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var result = $"Rental Record for {Name}\n";

            // split this for in 3 SRP. then use LINQ Tip:string.Concat
            foreach (var rental in _rentals)
            {
                var rentalAmount = rental.DetermineRentalAmount();

                frequentRenterPoints += rental.FrequentRenterPoints;

                result += $"\t{rental.Movie.Title}\t{rentalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";

                totalAmount += rentalAmount;
            }

            // add footer lines
            result += $"Amount owed is {totalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
            result += $"You earned {frequentRenterPoints} frequent renter points\n";

            return result;
        }
    }
}

