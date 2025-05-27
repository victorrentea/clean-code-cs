namespace Victor.Training.Cleancode.VideoStore;

public record Rental(Movie Movie, int NumberOfDays)
{
    public bool IsEligibleForBonus => Movie.MovieType == MovieType.NewRelease && NumberOfDays > 1;
    public decimal Amount()
    {
        var amount = 0m;
        switch (Movie.MovieType)
        {
            case MovieType.Regular:
                amount += 2;
                if (NumberOfDays > 2)
                {
                    amount += (NumberOfDays - 2) * 1.5m;
                }
                break;
            case MovieType.NewRelease:
                amount += NumberOfDays * 3;
                break;
            case MovieType.Children:
                amount += 1.5m;
                if (NumberOfDays > 3)
                {
                    amount += (NumberOfDays - 3) * 1.5m;
                }
                break;
        }

        return amount;
    }

}