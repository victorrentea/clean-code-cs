using clean_code_cs;
using System.Globalization;
using System.Text;

namespace Victor.Training.Cleancode.VideoStore
{
    public record Movie(string Title, PriceCode PriceCode);

    public record Rental(Movie Movie, int DaysOfRental)
    {
        private bool IsBonusEligible => Movie.PriceCode == PriceCode.NewRelease && DaysOfRental > 1;

        public decimal CalculateAmount() => Movie.PriceCode switch
        {
            PriceCode.Regular => CalculateRegularAmount(),
            PriceCode.NewRelease => CalculateNewReleaseAmount(),
            PriceCode.Childrens => CalculateChildrensAmount(),
            _ => throw new NotImplementedException(),
        };

        public string GetRentalLine() =>
            $"\t{Movie.Title}\t{CalculateAmount().ToString("0.0", CultureInfo.InvariantCulture)}\n";

        private decimal CalculateChildrensAmount() => ExtraAmount(3) + 1.5m;

        private decimal ExtraAmount(int limit)
        {
            var thisAmount = 0m;
            if (DaysOfRental > limit)
            {
                thisAmount += (DaysOfRental - limit) * 1.5m;
            }

            return thisAmount;
        }

        private decimal CalculateRegularAmount() => ExtraAmount(2) + 2;

        private decimal CalculateNewReleaseAmount() => DaysOfRental * 3;

        public int CalculateFrequentPoints() => IsBonusEligible ? 2 : 1;
    }

    public class Customer
    {
        private readonly List<Rental> _rentals = new();
        private string Name { get; }

        public Customer(string name) => Name = name;

        public void AddRental(Rental rental)
        {
            _rentals.Add(rental);
        }

        public string Statement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var statementBuilder = new StringBuilder().Append("Rental Record for " + Name + "\n");
            
            foreach (var rental in _rentals)
            {
                frequentRenterPoints += rental.CalculateFrequentPoints();
                statementBuilder.Append(rental.GetRentalLine());
                totalAmount += rental.CalculateAmount();
            }

            AddFooterLines(frequentRenterPoints, totalAmount, statementBuilder);

            return statementBuilder.ToString();
        }

        private static void AddFooterLines(int frequentRenterPoints, decimal totalAmount, StringBuilder statementBuilder)
        {
            statementBuilder.Append(GetAmountOwedStatement(totalAmount));
            statementBuilder.Append(GetFrequentRenterPointsLine(frequentRenterPoints));
        }

        private static string GetFrequentRenterPointsLine(int frequentRenterPoints) =>
            $"You earned {frequentRenterPoints} frequent renter points\n";

        private static string GetAmountOwedStatement(decimal totalAmount) =>
            $"Amount owed is {totalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
    }
}