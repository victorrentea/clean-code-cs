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

            foreach (var movie in _rentals)
            {
                var priceAmount = 0m;

                //dtermines the amount for each line
                switch (movie.Item1.PriceCode)
                {
                    case PriceCode.Regular:
                        priceAmount += 2;
                        if (movie.DaysRented > 2)
                        {
                            priceAmount += (movie.DaysRented - 2) * 1.5m;
                        }
                        break;
                    case PriceCode.NewRelease:
                        priceAmount += movie.DaysRented * 3;
                        break;
                    case PriceCode.Childrens:
                        priceAmount += 1.5m;
                        if (movie.DaysRented > 3)
                        {
                            priceAmount += (movie.DaysRented - 3) * 1.5m;
                        }
                        break;
                }

                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (movie.Item1.PriceCode == PriceCode.NewRelease
                    && movie.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                result += "\t" + movie.Item1.Title + "\t" + priceAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
                totalAmount += priceAmount;
            }

            // add footer lines
            result += "Amount owed is " + totalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + frequentRenterPoints.ToString() + " frequent renter points\n";

            return result;
        }

    }
}

