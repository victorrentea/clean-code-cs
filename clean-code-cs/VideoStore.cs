using System.Globalization;

namespace Victor.Training.Cleancode.VideoStore
{
    public record Customer (string Name)
    {
        private readonly List<Rental> _rentals = [];
        private int FrequentRenterPoints => _rentals.Count + _rentals.Count(x => x.IsEligibleForBonus);
        private decimal TotalAmount => _rentals.Sum(rental => rental.Amount);
        private string Body => string.Concat(_rentals.Select(GetBodyLine));

        public void AddRental(Movie movie, int numberOfDays)
        {
            _rentals.Add(new Rental(movie, numberOfDays));
        }

        public string GetStatement()
        {
            var result = "Rental Record for " + Name + "\n";

            result += Body;

            result += "Amount owed is " + TotalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + FrequentRenterPoints + " frequent renter points\n";

            return result;
        }

        private static string GetBodyLine(Rental rental)
        {
            return "\t" + rental.Movie.Title + "\t" + rental.Amount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
        }
    }
}