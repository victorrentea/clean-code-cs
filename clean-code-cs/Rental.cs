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

    //presentation ? MVC says separate presentation from core domain logic
    public string GetRentalLine() => $"\t{Movie.Title}\t{CalculateAmount().ToString("0.0", CultureInfo.InvariantCulture)}\n";

    public int CalculateFrequentPoints()
    {
        var points = 1;
        if (IsBonusEligible)
        {
            points++;// 1 more point ~> closer to the requirements = easier to maintain
        }
        return points;
        //return IsBonusEligible ? 2 : 1;
    }

    // Extra-DRY code: "accidental coupling"; It's (probably) just an accident both multiply with 1.5
    // "extract by coincidence"=bad

    // DRY up code (extract common things) only if THEY WILL CHANGE TOGHER

    private decimal CalculateChildrensAmount() => ExtraAmount(3) + 1.5m;

    private decimal ExtraAmount(int minimumDaysOfRent) =>
        DaysOfRental > minimumDaysOfRent ? (DaysOfRental - minimumDaysOfRent) * 1.5m : 0m;

    private decimal CalculateRegularAmount() => ExtraAmount(2) + 2;

    private decimal CalculateNewReleaseAmount() => DaysOfRental * 3;
}