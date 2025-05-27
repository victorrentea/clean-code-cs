using NUnit.Framework;
using Victor.Training.Cleancode.VideoStore;

namespace clean_code_cs_test
{
    internal class RentalTests
    {

        [TestCase("Star Wars", MovieType.NewRelease, 6, 18.0)]
 //       [TestCase("Star Wars", MovieType.NewRelease, 6, 18.0)]
        public void AmountCalculationTest(string movieName, MovieType movieType, int days, decimal expectedAmount) 
        { 
            var movie = new Movie(movieName, movieType);    
            var rental = new Rental(movie, days);
            var amount = rental.Amount;
            Assert.AreEqual(expectedAmount, amount);
        }
    }
}
