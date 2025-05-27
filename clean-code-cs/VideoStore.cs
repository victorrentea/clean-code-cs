using System.Globalization;
using static Victor.Training.Cleancode.VideoStore.Customer;

namespace Victor.Training.Cleancode.VideoStore
{
    public class Movie
    {
        public MovieType MovieType { get; set; }
        public string Title { get; }

        public Movie(string title, MovieType movieType)
        {
            Title = title;
            MovieType = movieType;
        }
    }

    public class Customer
    {
        private readonly List<Rental> _rentals = new();
        public string Name { get; }
        public int FrequentRenterPoints => _rentals.Count + _rentals.Where(x => x.IsEligibleForBonus).Count();

        public Customer(string name)
        {
            Name = name;
        }

        public record Rental(Movie Movie, int NumberOfDays)
        {
            public bool IsEligibleForBonus => Movie.MovieType == MovieType.NewRelease && NumberOfDays > 1;
            public decimal Amount()
            {
                var amount = 0m;
                switch (Movie.MovieType)
                {
                    case MovieType.Regular:
                        amount += 2;
                        if (NumberOfDays > 2)
                        {
                            amount += (NumberOfDays - 2) * 1.5m;
                        }
                        break;
                    case MovieType.NewRelease:
                        amount += NumberOfDays * 3;
                        break;
                    case MovieType.Children:
                        amount += 1.5m;
                        if (NumberOfDays > 3)
                        {
                            amount += (NumberOfDays - 3) * 1.5m;
                        }
                        break;
                }

                return amount;
            }

        }

        public void AddRental(Movie movie, int numberOfDays)
        {
            _rentals.Add(new Rental(movie, numberOfDays));
        }

        public string Statement()
        {
            var totalAmount = CalculateTotalAmount();

            var result = "Rental Record for " + Name + "\n";
            foreach (var rental in _rentals) { 
                result += "\t" + rental.Movie.Title + "\t" + rental.Amount().ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            }

            // add footer lines
            result += "Amount owed is " + totalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + FrequentRenterPoints.ToString() + " frequent renter points\n";

            return result;
        }

        private decimal CalculateTotalAmount()
        {
            var totalAmount = 0m;
            foreach (var rental in _rentals)
            {
                //dtermines the amount for each line
                var rentalAmount = rental.Amount();

                totalAmount += rentalAmount;
            }
            return totalAmount;
        }
    }
}

