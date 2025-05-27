using clean_code_cs;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;

namespace Victor.Training.Cleancode.VideoStore
{
    public record Movie(string Title, PriceCode PriceCode);

    public class Customer
    {
        private readonly List<(Movie,int)> _rentals = new();
        private string Name { get; }

        public Customer(string name) => Name = name;

        public void AddRental(Movie movie, int d)
        {
            _rentals.Add((movie, d));
        }

        public string Statement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;            
            var statementBuilder = new StringBuilder().Append("Rental Record for " + Name + "\n");
            foreach (var rental in _rentals)
            {
                var thisAmount = CalculateAmount(rental.Item1, rental.Item2);

                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (IsBonusEligibleRental(rental.Item1, rental.Item2))
                {
                    frequentRenterPoints++;
                }

                statementBuilder.Append(GetRentalLine(rental.Item1, thisAmount));
                totalAmount += thisAmount;
            }

            // add footer lines
            statementBuilder.Append(GetAmountOwedStatement(totalAmount));
            statementBuilder.Append(GetFrequentRenterPointsLine(frequentRenterPoints));

            return statementBuilder.ToString();

        }

        private static decimal CalculateAmount(Movie movie, int dr)
        {
            var thisAmount = 0m;

            //determines the amount for each line
            switch (movie.PriceCode)
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


        private static bool IsBonusEligibleRental(Movie movie, int dr)
        {
            return movie.PriceCode == PriceCode.NewRelease && dr > 1;
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

