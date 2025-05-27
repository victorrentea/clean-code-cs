using clean_code_cs;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;

namespace Victor.Training.Cleancode.VideoStore
{
    public record Movie(string Title, PriceCode PriceCode);

    public record Rental(Movie Movie, int DaysOfRental)
    {
        public bool IsBonusEligible => Movie.PriceCode == PriceCode.NewRelease && DaysOfRental > 1;
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
                var thisAmount = CalculateAmount(rental.Movie.PriceCode, rental.DaysOfRental);

                frequentRenterPoints++;

                if (rental.IsBonusEligible)
                {
                    frequentRenterPoints++;
                }

                statementBuilder.Append(GetRentalLine(rental.Movie, thisAmount));
                totalAmount += thisAmount;
            }

            AddFooterLines(frequentRenterPoints, totalAmount, statementBuilder);

            return statementBuilder.ToString();

        }

        private static void AddFooterLines(int frequentRenterPoints, decimal totalAmount, StringBuilder statementBuilder)
        {
            statementBuilder.Append(GetAmountOwedStatement(totalAmount));
            statementBuilder.Append(GetFrequentRenterPointsLine(frequentRenterPoints));
        }

        private static decimal CalculateAmount(PriceCode priceCode, int dr)
        {
            var thisAmount = 0m;

            //determines the amount for each line
            switch (priceCode)
            {
                case PriceCode.Regular:
                    thisAmount = CalculateRegularAmount(thisAmount, dr);
                    break;
                case PriceCode.NewRelease:
                    thisAmount = CalculateNewReleaseAmount(thisAmount, dr);
                    break;
                case PriceCode.Childrens:
                    thisAmount = CalculateChildrensAmount(thisAmount, dr);
                    break;
            }

            return thisAmount;
        }

        private static decimal CalculateChildrensAmount(decimal thisAmount, int dr)
        {
            thisAmount += 1.5m;
            thisAmount = ExtraAmount(thisAmount, dr, 3);

            return thisAmount;
        }

        private static decimal ExtraAmount(decimal thisAmount, int dr, int limit)
        {
            if (dr > limit)
            {
                thisAmount += (dr - limit) * 1.5m;
            }

            return thisAmount;
        }

        private static decimal CalculateRegularAmount(decimal thisAmount, int dr)
        {
            thisAmount += 2;
            thisAmount = ExtraAmount(thisAmount, dr, 2);

            return thisAmount;
        }

        private static decimal CalculateNewReleaseAmount(decimal thisAmount, int dr)
        {
            thisAmount += dr * 3;
            return thisAmount;
        }

        private static string GetRentalLine(Movie movie, decimal thisAmount)
        {
            return $"\t{movie.Title}\t{thisAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
        }

        private static string GetFrequentRenterPointsLine(int frequentRenterPoints)
        {            
            return $"You earned {frequentRenterPoints} frequent renter points\n";                
        }

        private static string GetAmountOwedStatement(decimal totalAmount)
        {
            return $"Amount owed is {totalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
        }
    }
}

