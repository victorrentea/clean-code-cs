using System;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    public class PrimitiveObsession
    {
        public static void Main(string[] args)
        {
            new PrimitiveObsession().HandlePrimitiveObsession("CARD");
        }

        public Dictionary<long, Dictionary<string, int>> FetchData(string paymentMethod)
        {
            long customerId = 1L;
            int product1Count = 2;
            int product2Count = 4;
            return new Dictionary<long, Dictionary<string, int>>
            {
                { customerId, new Dictionary<string, int>
                    {
                        { "Table", product1Count },
                        { "Chair", product2Count }
                    }
                }
            };
        }


        public void HandlePrimitiveObsession(string paymentMethod)
        {
            if (paymentMethod != "CARD" && paymentMethod != "CASH")
            {
                throw new ArgumentException("Only CARD payment method is supported");
            }

            var map = FetchData(paymentMethod);

            foreach (var e in map)
            {
                string pl = string.Join(", ", e.Value.Select(entry => $"{entry.Value} pcs. of {entry.Key}"));
                Console.WriteLine($"cid={e.Key} got {pl}");
            }
        }
    }
}
