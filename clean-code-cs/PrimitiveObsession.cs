using System;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    public enum PaymentMethod
    {
        CARD, CASH, SHEEP, BLOOD
    }
    public class PrimitiveObsession
    {
        public static void Main(string[] args)
        {
            String s = "CARd ";
            //PaymentType paymentType = (PaymentType)Enum.Parse(typeof(PaymentType),s);
            PaymentMethod paymentType = Enum.Parse<PaymentMethod>(s.Trim().ToUpper());


            new PrimitiveObsession().HandlePrimitiveObsession(paymentType);
        }

        //public Dictionary<CustomerId, List<ProductCount>> FetchData(string paymentMethod)
        public Dictionary<CustomerId, Dictionary<string, int>> FetchData(PaymentMethod paymentMethod)
        {
            long customerId = 1L;
            int product1Count = 2;
            int product2Count = 4;
            return new Dictionary<CustomerId, Dictionary<string, int>>
            {
                { new CustomerId(customerId), new Dictionary<string, int>
                    {
                        { "Table", product1Count },
                        { "Chair", product2Count }
                    }
                }
            };
        }


        public void HandlePrimitiveObsession(PaymentMethod paymentMethod)
        {
            if (paymentMethod != PaymentMethod.CARD && paymentMethod != PaymentMethod.CASH)
            {
                throw new ArgumentException("Only CARD payment method is supported");
            }

            //Dictionary<CustomerId, Dictionary<string, int>> dict = FetchData(paymentMethod);
            var boughtItemCounts = FetchData(paymentMethod);

            foreach (var e in boughtItemCounts)
            {
                string pl = string.Join(", ", e.Value.Select(entry => $"{entry.Value} pcs. of {entry.Key}"));
                Console.WriteLine($"cid={e.Key.Id} got {pl}");
            }
        }
    }
    public readonly record struct CustomerId(long Id); // typealias
    public readonly record struct ProductCount(string ProductName, uint Count); // typealias
}
