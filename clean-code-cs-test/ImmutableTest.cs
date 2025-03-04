using System;
using NUnit.Framework;

namespace Victor.Training.Cleancode
{
	public class ImmutableTest
	{
        [Test]
        public void ThrowsExceptionForBonusPackageValueLessThan10()
        {
            ImmutableAdvanced.Main();
        }
    }
}

