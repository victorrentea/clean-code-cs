using System;
using System.Collections.Generic;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Victor.Training.Cleancode
{
    class GuardsTest
    {
        private readonly Guards guards = new Guards();

        [Test]
        public void t()
        {
            
            void f(Int32 x) 
            {
                x++;
                Console.WriteLine("inside " + x);
            }

            Int32 i = 3;
            Console.WriteLine("Before " + i);
            f(i);
            Console.WriteLine("After " + i);
        }

        // generated via a 10 minutes chat with GitHub Copilot
        [Test]
        public void ThrowsExceptionForBonusPackageValueLessThan10()
        {
            Marine marine = new Marine(false, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(9);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }

        [Test]
        public void ThrowsExceptionForInvalidBonusPackageValueTooLarge()
        {
            Marine marine = new Marine(false, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(101);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }

        [Test]
        public void ThrowsExceptionForNullMarine()
        {
            Marine marine = null;
            BonusPackage bonusPackage = new BonusPackage(50);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }

        [Test]
        public void ThrowsExceptionForInvalidBonusPackageValue()
        {
            Marine marine = new Marine(false, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(200);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }

        [Test]
        public void CalculatesPayAmountForActiveMarine()
        {
            Marine marine = new Marine(false, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(50);

            int payAmount = guards.GetPayAmount(marine, bonusPackage);

            That(payAmount, Is.EqualTo(1050));
        }

        [Test]
        public void CalculatesPayAmountForMarineWithThreeAwards()
        {
            Award award = new Award();
            Marine marine = new Marine(false, false, 10, new List<Award> { award, award, award });
            BonusPackage bonusPackage = new BonusPackage(50);

            int payAmount = guards.GetPayAmount(marine, bonusPackage);

            That(payAmount, Is.EqualTo(4050));
        }

        [Test]
        public void CalculatesPayAmountForRetiredMarine()
        {
            Marine marine = new Marine(false, true, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(50);

            int payAmount = guards.GetPayAmount(marine, bonusPackage);

            That(payAmount, Is.EqualTo(2));
        }

        [Test]
        public void CalculatesPayAmountForMarineWithAwards()
        {
            Award award = new Award();
            Marine marine = new Marine(false, false, 10, new List<Award> { award });
            BonusPackage bonusPackage = new BonusPackage(50);

            int payAmount = guards.GetPayAmount(marine, bonusPackage);

            That(payAmount, Is.EqualTo(2050));
        }

        [Test]
        public void CalculatesPayAmountForDeadMarine()
        {
            Marine marine = new Marine(true, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(50);

            int payAmount = guards.GetPayAmount(marine, bonusPackage);

            That(payAmount, Is.EqualTo(Guards.DEAD_PAY_AMOUNT));
        }

        [Test]
        public void ThrowsExceptionForMarineWithNullYearsService()
        {
            Marine marine = new Marine(false, false, null, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(50);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }

        [Test]
        public void ThrowsExceptionForInvalidBonusPackage()
        {
            Marine marine = new Marine(false, false, 10, new List<Award>());
            BonusPackage bonusPackage = new BonusPackage(200);

            Throws<ArgumentException>(() => guards.GetPayAmount(marine, bonusPackage));
        }
    }
}

