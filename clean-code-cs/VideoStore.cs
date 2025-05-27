using System.Globalization;

namespace Victor.Training.Cleancode.VideoStore
{
    public enum PriceCode
    {
        Regular,
        NewRelease,
        Childrens
    }

    public record Movie(string Title, PriceCode PriceCode);

    public class Customer
    {
        

        private readonly List<(Movie Movie, int DaysRented)> _rentals = new();
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Movie movie, int daysRented)
        {
            _rentals.Add((movie, daysRented));
        }

        public string PrintStatement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var result = $"Rental Record for {Name}\n";

            foreach (var rental in _rentals)
            {
                var rentalAmount = DetermineRentalAmount(rental);
                frequentRenterPoints = AddFrequentRenterPoints(frequentRenterPoints, rental);

                result += $"\t{rental.Movie.Title}\t{rentalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
                totalAmount += rentalAmount;
            }

            // add footer lines
            result += $"Amount owed is {totalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
            result += $"You earned {frequentRenterPoints} frequent renter points\n";

            return result;
        }

        private static int AddFrequentRenterPoints(int frequentRenterPoints, (Movie Movie, int DaysRented) rental)
        {
            frequentRenterPoints++;

            if (rental.Movie.PriceCode == PriceCode.NewRelease
                && rental.DaysRented > 1)
            {
                frequentRenterPoints++;
            }

            return frequentRenterPoints;
        }

        private static decimal DetermineRentalAmount((Movie Movie, int DaysRented) rental)
        {
            const decimal RegularPriceBase = 2;
            const decimal RegularPriceExtraDailyRate = 1.5m;
            const int RegularPriceExtraDaysThreshold = 2;

            const int NewReleaseDailyRate = 3;

            const decimal ChildrensPriceBase = 1.5m;
            const int ChildrensPriceExtraDaysThreshold = 3;
            const decimal ChildrensPriceExtraDailyRate = 1.5m;

            decimal rentalAmount = 0m;

            switch (rental.Movie.PriceCode)
            {
                case PriceCode.Regular:
                    rentalAmount += RegularPriceBase;
                    if (rental.DaysRented > RegularPriceExtraDaysThreshold)
                    {
                        rentalAmount += (rental.DaysRented - RegularPriceExtraDaysThreshold) * RegularPriceExtraDailyRate;
                    }
                    break;
                case PriceCode.NewRelease:
                    rentalAmount += rental.DaysRented * NewReleaseDailyRate;
                    break;
                case PriceCode.Childrens:
                    rentalAmount += ChildrensPriceBase;
                    if (rental.DaysRented > ChildrensPriceExtraDaysThreshold)
                    {
                        rentalAmount += (rental.DaysRented - ChildrensPriceExtraDaysThreshold) * ChildrensPriceExtraDailyRate;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rental.Movie.PriceCode));
            }

            return rentalAmount;
        }
    }
}

