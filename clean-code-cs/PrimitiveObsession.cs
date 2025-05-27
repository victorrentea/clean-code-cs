using System;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    //test push
    public enum PaymentMethod
    {
        CARD,
        CASH,
        BLOOD,
        SHEEP,
        COUPONS
    }
    public class PrimitiveObsession
    {
        public static void Main(string[] args)
        {
            new PrimitiveObsession().HandlePrimitiveObsession(PaymentMethod.CARD);
        }


        //public Dictionary<CustomerId, Dictionary<ProductName, Quantity>>
        public Dictionary<long, Dictionary<string, int>> FetchData(PaymentMethod paymentMethod)
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

        //microtypes for VERY CRITICAL IDS: SSN, IBAN, Email, SWIFT, VATCode, OrderId
        public record CustomerId(long Value);
        public record ProductName(string Value);
        public record Quantity(int Value);


        public void HandlePrimitiveObsession(PaymentMethod paymentMethod)
        {
            //if (paymentMethod != PaymentMethod.CARD && paymentMethod != PaymentMethod.CASH && paymentMethod!=PaymentMethod.SHEEP)
            //if (! (paymentMethod is PaymentMethod.CARD or PaymentMethod.CASH or PaymentMethod.SHEEP))
            var supportedPaymentMethods = new HashSet<PaymentMethod> { PaymentMethod.CARD, PaymentMethod.CASH, PaymentMethod.SHEEP };
            if (!supportedPaymentMethods.Contains(paymentMethod))
                throw new ArgumentException($"{paymentMethod} not supported");

            // use 
            var customerProductCounts = FetchData(paymentMethod);

            foreach (var e in customerProductCounts)
            {
                string pl = string.Join(", ", e.Value.Select(entry => $"{entry.Value} pcs. of {entry.Key}"));
                Console.WriteLine($"cid={e.Key} got {pl}");
            }
        }
    }
}
