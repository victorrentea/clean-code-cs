using System.Globalization;

namespace clean_code_cs;

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

    public string GetRentalLine() => $"\t{Movie.Title}\t{CalculateAmount().ToString("0.0", CultureInfo.InvariantCulture)}\n";

    public int CalculateFrequentPoints() => IsBonusEligible ? 2 : 1;

    private decimal CalculateChildrensAmount() => ExtraAmount(3) + 1.5m;

    private decimal ExtraAmount(int minimumDaysOfRent) => DaysOfRental > minimumDaysOfRent ? (DaysOfRental - minimumDaysOfRent) * 1.5m : 0m;

    private decimal CalculateRegularAmount() => ExtraAmount(2) + 2;

    private decimal CalculateNewReleaseAmount() => DaysOfRental * 3;
}