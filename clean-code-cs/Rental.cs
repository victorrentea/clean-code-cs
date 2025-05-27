namespace Victor.Training.Cleancode.VideoStore;

public record Rental(Movie Movie, int NumberOfDays)
{
    public bool IsEligibleForBonus => Movie.MovieType == MovieType.NewRelease && NumberOfDays > 1;

    public decimal Amount => Movie.MovieType switch
    {
        MovieType.Regular => 2 + (NumberOfDays > 2 ? (NumberOfDays - 2) * 1.5m : 0),
        MovieType.NewRelease => NumberOfDays * 3,
        MovieType.Children => 1.5m + (NumberOfDays > 3 ? (NumberOfDays - 3) * 1.5m : 0),
        _ => throw new NotImplementedException($"{Movie.MovieType} amount is not defined")
    };
}