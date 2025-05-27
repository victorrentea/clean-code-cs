using System;
using Victor.Training.Cleancode.VideoStore;
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
            customer.AddRental(new Movie("Star Wars", Movie.NEW_RELEASE), 6);
            customer.AddRental(new Movie("Sofia", Movie.CHILDRENS), 7);
            customer.AddRental(new Movie("Inception", Movie.REGULAR), 5);
            customer.AddRental(new Movie("Wicked", Movie.CHILDRENS), 3);

            var expected =
                "Rental Record for John Doe\n" +
                "\tStar Wars\t18.0\n" +
                "\tSofia\t7.5\n"+ 
                "\tInception\t6.5\n" +
                "\tWicked\t1.5\n" +
                "Amount owed is 33.5\n" +
                "You earned 5 frequent renter points\n";

            Assert.That(Normalize(customer.Statement()), Is.EqualTo(Normalize(expected)));
        }
    }
}

