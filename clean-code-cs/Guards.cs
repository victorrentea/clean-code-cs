namespace Victor.Training.Cleancode
{
    public class Guards
    {
        public static readonly int DEAD_PAY_AMOUNT = 1;

        public int GetPayAmount(Marine marine, BonusPackage bonusPackage)
        {
            int result;
            if (marine != null && !(bonusPackage.Value < 10 || bonusPackage.Value > 100))
            {
                if (!marine.Dead)
                {
                    if (!marine.Retired)
                    {
                        if (marine.YearsOfService != null)
                        {
                            result = marine.YearsOfService.Value * 100 + bonusPackage.Value;
                            if (marine.Awards.Count > 0)
                            {
                                result += 1000;
                            }
                            if (marine.Awards.Count >= 3)
                            {
                                result += 2000;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Any marine should have the years of service set");
                        }
                    }
                    else result = RetiredAmount();
                }
                else
                {
                    result = DEAD_PAY_AMOUNT;
                }
            }
            else
            {
                throw new ArgumentException("Not applicable!");
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

