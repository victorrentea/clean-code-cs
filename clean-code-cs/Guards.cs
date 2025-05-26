namespace Victor.Training.Cleancode
{
    public class Guards
    {
        private const int SECONDS_IN_AN_HOUR = 17280;
        public static readonly int DEAD_PAY_AMOUNT = 1;

        public int GetPayAmount(Marine marine, BonusPackage bonusPackage)
        {
            if (marine == null || bonusPackage.Value < 10 || bonusPackage.Value > 100)
            {
                throw new ArgumentException("Not applicable!");
            }

            if (marine.Dead)
            {
                return DEAD_PAY_AMOUNT;
            }

            if (marine.Retired)
                return RetiredAmount();
             
            if (marine.YearsOfService == null)
            {
                throw new ArgumentException("Any marine should have the years of service set");
            }

            var someVar = 1;
            if (someVar > SECONDS_IN_AN_HOUR)
            {

            }

            int result = marine.YearsOfService.Value * 100 + bonusPackage.Value;
            if (marine.Awards.Count > 0)
            {
                result += 1000;
            }
            if (marine.Awards.Count >= 3)
            {
                result += 2000;
            }
           
            return result;
        }

        private int RetiredAmount()
        {
            return 2;
        }
    }

    public record Marine(bool Dead, bool Retired, int? YearsOfService, List<Award> Awards)
    {
    }

    public record BonusPackage(int Value)
    {
    }

    public class Award
    {
    }
}

