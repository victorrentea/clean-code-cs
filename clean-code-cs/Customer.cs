using System.Globalization;
using System.Text;

namespace clean_code_cs;

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
        var statementBuilder = new StringBuilder().Append("Rental Record for " + Name + "\n");

        var frequentRenterPoints = _rentals.Sum(rental => rental.CalculateFrequentPoints());

        _rentals.ForEach(x => statementBuilder.Append(x.GetRentalLine()));

        var totalAmount = _rentals.Sum(rental => rental.CalculateAmount());

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