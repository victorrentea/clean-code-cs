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

    public class Rental
    {
        private bool TwoDayNewReleaseRental => Movie.PriceCode == PriceCode.NewRelease && DaysRented > 1;

        public Movie Movie { get; }
        public int DaysRented { get; }

        public int FrequentRenterPointsToAdd => TwoDayNewReleaseRental ? 2 : 1;

        public Rental(Movie movie, int daysRented)
        {
            Movie = movie;
            DaysRented = daysRented;
        }

        public decimal DetermineRentalAmount()
        {
            const decimal RegularPriceBase = 2;
            const decimal RegularPriceExtraDailyRate = 1.5m;
            const int RegularPriceExtraDaysThreshold = 2;

            const int NewReleaseDailyRate = 3;

            const decimal ChildrensPriceBase = 1.5m;
            const decimal ChildrensPriceExtraDailyRate = 1.5m;
            const int ChildrensPriceExtraDaysThreshold = 3;

            var rentalAmount = 0m;

            switch (Movie.PriceCode)
            {
                case PriceCode.Regular:
                    rentalAmount += RegularPriceBase;
                    if (DaysRented > RegularPriceExtraDaysThreshold)
                    {
                        rentalAmount += (DaysRented - RegularPriceExtraDaysThreshold) * RegularPriceExtraDailyRate;
                    }

                    break;
                case PriceCode.NewRelease:
                    rentalAmount += DaysRented * NewReleaseDailyRate;

                    break;
                case PriceCode.Children:
                    rentalAmount += ChildrensPriceBase;
                    if (DaysRented > ChildrensPriceExtraDaysThreshold)
                    {
                        rentalAmount += (DaysRented - ChildrensPriceExtraDaysThreshold) * ChildrensPriceExtraDailyRate;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Movie.PriceCode));
            }

            return rentalAmount;
        }
    }

    public class Customer
    {
        private readonly List<Rental> _rentals = new();

        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Movie movie, int daysRented)
        {
            _rentals.Add(new Rental(movie, daysRented));
        }

        public string PrintStatement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var result = $"Rental Record for {Name}\n";

            foreach (var rental in _rentals)
            {
                var rentalAmount = rental.DetermineRentalAmount();

                frequentRenterPoints += rental.FrequentRenterPointsToAdd;

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

