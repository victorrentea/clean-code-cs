using NUnit.Framework;

namespace Victor.Training.Cleancode.VideoStore.Tests
{
    public class VideoStoreTests
    {
        private static string Normalize(string s) => s.Replace("\r\n", "\n").Replace("\r", "\n");

        [Test]
        public void CharacterizationTest()
        {
            var customer = new Customer("John Doe");
            customer.AddRental(new Movie("Star Wars", PriceCode.NewRelease), 6);
            customer.AddRental(new Movie("Sofia", PriceCode.Children), 7);
            customer.AddRental(new Movie("Inception", PriceCode.Regular), 5);
            customer.AddRental(new Movie("Wicked", PriceCode.Children), 3);

            const string expected =
                "Rental Record for John Doe\n" +
                "\tStar Wars\t18.0\n" +
                "\tSofia\t7.5\n"+
                "\tInception\t6.5\n" +
                "\tWicked\t1.5\n" +
                "Amount owed is 33.5\n" +
                "You earned 5 frequent renter points\n";

            Assert.That(Normalize(customer.PrintStatement()), Is.EqualTo(Normalize(expected)));
        }
    }
}
