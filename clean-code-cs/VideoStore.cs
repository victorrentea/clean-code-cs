using System;
using System.Globalization;
using System.Collections.Generic;

namespace Victor.Training.Cleancode.VideoStore
{

    public class Movie
    {
        public const int REGULAR = 0;
        public const int NEW_RELEASE = 1;
        public const int CHILDRENS = 2;

        public int PriceCode { get; set; }
        public virtual string Title { get; }

        public Movie(string title, int priceCode)
        {
            Title = title;
            PriceCode = priceCode;
        }
    }  

    public class Customer
    {
        private readonly List<(Movie,int)> _rentals = new();
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Movie movie, int d)
        {
            _rentals.Add((movie, d));
        }

        public string Statement()
        {
            var frequentRenterPoints = 0;
            var totalAmount = 0m;
            var result = "Rental Record for " + Name + "\n";
            foreach (var each in _rentals)
            {
                var thisAmount = 0m;
                var dr = each.Item2;
                //dtermines the amount for each line
                switch (each.Item1.PriceCode)
                {
                    case Movie.REGULAR:
                        thisAmount += 2;
                        if (dr > 2)
                        {
                            thisAmount += (dr - 2) * 1.5m;
                        }
                        break;
                    case Movie.NEW_RELEASE:
                        thisAmount += dr * 3;
                        break;
                    case Movie.CHILDRENS:
                        thisAmount += 1.5m;
                        if (dr > 3)
                        {
                            thisAmount += (dr - 3) * 1.5m;
                        }
                        break;
                }

                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (each.Item1.PriceCode == Movie.NEW_RELEASE
                    && dr > 1)
                {
                    frequentRenterPoints++;
                }

                result += "\t" + each.Item1.Title + "\t" + thisAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
                totalAmount += thisAmount;
            }

            // add footer lines
            result += "Amount owed is " + totalAmount.ToString("0.0", CultureInfo.InvariantCulture) + "\n";
            result += "You earned " + frequentRenterPoints.ToString() + " frequent renter points\n";

            return result;
        }

    }
}

