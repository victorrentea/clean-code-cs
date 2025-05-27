using System.Globalization;
using static Victor.Training.Cleancode.VideoStore.Customer;

namespace Victor.Training.Cleancode.VideoStore
{

    public record Movie(string Title, MovieType MovieType);

    public class Customer
    {
        private readonly List<Rental> _rentals = new();
        public string Name { get; }
        public int FrequentRenterPoints => _rentals.Count + _rentals.Count(x => x.IsEligibleForBonus);
        public decimal TotalAmount => _rentals.Sum(rental => rental.Amount());

        public Customer(string name)
        {
            Name = name;
        }

        

        public void AddRental(Movie movie, int numberOfDays)
        {
            _rentals.Add(new Rental(movie, numberOfDays));
        }

        public string Statement()
        {

            var result = "Rental Record for " + Name + "\n";
            result += GetBody();

            // add footer lines
            result += "Amount owed is " + TotalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + FrequentRenterPoints + " frequent renter points\n";

            return result;
        }

        private string GetBody()
        {
            var result = string.Empty;
            foreach (var rental in _rentals)
            {
                result += "\t" + rental.Movie.Title + "\t" + rental.Amount().ToString("0.0", CultureInfo.InvariantCulture) +
                          "\n";
            }

            return result;
        }
    }
}

