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
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

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
                var thisAmount = 0m;
                var dr = rental.Item2;
                //dtermines the amount for each line
                switch (rental.Item1.PriceCode)
                {
                    case PriceCode.Regular:
                        thisAmount += 2;
                        if (dr > 2)
                        {
                            thisAmount += (dr - 2) * 1.5m;
                        }
                        break;
                    case PriceCode.NewRelease:
                        thisAmount += dr * 3;
                        break;
                    case PriceCode.Childrens:
                        thisAmount += 1.5m;
                        if (dr > 3)
                        {
                            thisAmount += (dr - 3) * 1.5m;
                        }
                        break;
                }

                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (IsBonusEligibleRental(rental, dr))
                {
                    frequentRenterPoints++;
                }

                statementBuilder.Append(GetRentalLine(rental, thisAmount));
                totalAmount += thisAmount;
            }

            // add footer lines
            statementBuilder.Append(GetAmountOwedStatement(totalAmount));
            statementBuilder.Append(GetFrequentRenterPointsLine(frequentRenterPoints));

            return statementBuilder.ToString();

        }

        private static bool IsBonusEligibleRental((Movie, int) each, int dr)
        {
            return each.Item1.PriceCode == PriceCode.NewRelease && dr > 1;
        }

        private static string GetRentalLine((Movie, int) each, decimal thisAmount)
        {
            return "\t" + each.Item1.Title + "\t" + thisAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
        }

        private static string GetFrequentRenterPointsLine(int frequentRenterPoints)
        {            
            return $"You earned {frequentRenterPoints} frequent renter points\n";                
        }

        private static string GetAmountOwedStatement(decimal totalAmount)
        {
            return "Amount owed is " + totalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
        }
    }
}

