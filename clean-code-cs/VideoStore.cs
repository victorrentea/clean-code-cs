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
        private const decimal RegularPriceBase = 2;
        private const decimal RegularPriceExtraDailyRate = 1.5m;
        private const int RegularPriceExtraDaysThreshold = 2;

        private const int NewReleaseDailyRate = 3;

        private const decimal ChildrensPriceBase = 1.5m;
        private const int ChildrensPriceExtraDaysThreshold = 3;
        private const decimal ChildrensPriceExtraDailyRate = 1.5m;

        private readonly List<(Movie Movie, decimal DaysRented)> _rentals = new();
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Movie movie, int daysRented)
        {
            _rentals.Add((movie, daysRented));
        }

        public string Statement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var result = $"Rental Record for {Name}\n";

            foreach (var rental in _rentals)
            {
                var priceAmount = 0m;

                //dtermines the amount for each line
                switch (rental.Movie.PriceCode)
                {
                    case PriceCode.Regular:
                        priceAmount += RegularPriceBase;
                        if (rental.DaysRented > RegularPriceExtraDaysThreshold)
                        {
                            priceAmount += (rental.DaysRented - RegularPriceExtraDaysThreshold) * RegularPriceExtraDailyRate;
                        }
                        break;
                    case PriceCode.NewRelease:
                        priceAmount += rental.DaysRented * NewReleaseDailyRate;
                        break;
                    case PriceCode.Childrens:
                        priceAmount += ChildrensPriceBase;
                        if (rental.DaysRented > ChildrensPriceExtraDaysThreshold)
                        {
                            priceAmount += (rental.DaysRented - ChildrensPriceExtraDaysThreshold) * ChildrensPriceExtraDailyRate;
                        }
                        break;
                }

                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.PriceCode == PriceCode.NewRelease
                    && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                result += "\t" + rental.Movie.Title + "\t" + priceAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
                totalAmount += priceAmount;
            }

            // add footer lines
            result += "Amount owed is " + totalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + frequentRenterPoints.ToString() + " frequent renter points\n";

            return result;
        }

    }
}

