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

    // avoid doing in-mem side-effects (not pure) after an =>
    //_rentals.ForEach(x => statementBuilder.Append(x.GetRentalLine()));

    public string Statement() => Header() + Body() + Footer();

    private string Body() => string.Concat(_rentals.Select(r => r.GetRentalLine()));

    private string Header() => "Rental Record for " + Name + "\n";

    private string Footer()
    {
        var totalPoints = _rentals.Sum(rental => rental.CalculateFrequentPoints());
        var totalAmount = _rentals.Sum(rental => rental.CalculateAmount());

        return GetAmountOwedStatement(totalAmount)
             + GetFrequentRenterPointsLine(totalPoints);
    }

    private static string GetFrequentRenterPointsLine(int frequentRenterPoints) =>
        $"You earned {frequentRenterPoints} frequent renter points\n";

    private static string GetAmountOwedStatement(decimal totalAmount) =>
        $"Amount owed is {totalAmount.ToString("0.0", CultureInfo.InvariantCulture)}\n";
}