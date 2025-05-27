using System;
namespace Victor.Training.Cleancode
{
    public class PureFunction
    {

        ICustomerRepo customerRepo;
        IThirdPartyPricesApi thirdPartyPricesApi;
        ICouponRepo couponRepo;
        IProductRepo productRepo;

        public PureFunction(ICustomerRepo customerRepo, IThirdPartyPricesApi thirdPartyPricesApi, ICouponRepo couponRepo, IProductRepo productRepo)
        {
            this.customerRepo = customerRepo;
            this.thirdPartyPricesApi = thirdPartyPricesApi;
            this.couponRepo = couponRepo;
            this.productRepo = productRepo;
        }


        //TODO extract most complexity into a pure function:
        //  to have clear the inputs
        //  to be testable without mocks
        public Dictionary<long, double> ComputePrices(
            long customerId,
            List<long> productIds,
            Dictionary<long, double> internalPrices)
        {
            Customer customer = customerRepo.FindById(customerId); // SELECT will throw if null Customer comes back if you enable null check per project

            List<Product> products = productRepo.FindAllById(productIds); // SELECT .. WHERE ID IN (?,?..?)

            //Q: what if products.size=1M. won't + 1 foreach hurt performance
            //A: NO. NEVER. because you already had it in memory in a List = loading that list took many seconds. Out of Memory?
            // compared to the time took to I/O
            //BTW: the correct way to process 1M products is using LINQ Streaming (never materialize the actual list).
            // then it IS important (ofc) not to traverse the data in DB twice!

            //Q: when can +1 foreach hurt performance, as measured in prod
            //A: Gaming: added to the main game loop in a Unity game, since the (decent) list would already be in memory, and + few ms matter
            //A: StockTrading: response times of 50 microseconds (us) 10**-6 server.
            //> in such cases usually memory allocation is discouraged as well (not allowed to 'new' objects)

            // From faculty we come with myth of O(N2) O(NlogN)
            Dictionary<long, double> initialPrices = ResolveInitialPrices(thirdPartyPricesApi, internalPrices, products);
            
            var result = ApplyCoupons(customer.Coupons, products, initialPrices);

            couponRepo.MarkUsedCoupons(customerId, result.UsedCoupons); // INSERT

            return result.FinalPrices;
        }

        // TODO perhaps a record
        //record class PriceCalculationResult(
        //    Dictionary<long, double> FinalPrices,
        //    List<Coupon> UsedCoupons);

        // this is a PURE function: same result

        // Victor is fine unit testing this COMPLEX PURE function independently w/o mocking
        // Data IN -> data out = best type of test strategy for complex logic
        // holy grail of unit testing => decouple high complexity
        // a) traditional: move to another class as a public method
        // b) hacky: open this method just for tests = "subcutaneous tests"
        internal static (Dictionary<long, double> FinalPrices, List<Coupon> UsedCoupons) ApplyCoupons(
            List<Coupon> coupons,
            List<Product> products,
            Dictionary<long, double> initialPrices)
        {
            List<Coupon> usedCoupons = new();
            Dictionary<long, double> finalPrices = new();
            foreach (Product product in products)
            {
                var price = initialPrices[product.Id];
                foreach (Coupon coupon in coupons)
                {
                    if (coupon.AutoApply && coupon.IsApplicableFor(product) && !usedCoupons.Contains(coupon))
                    {
                        price = coupon.Apply(product, price);
                        usedCoupons.Add(coupon); // TODO can we first select coupons to apply in PASS1, and in PASS2 apply them?
                    }
                }
                finalPrices[product.Id] = price;
            }
            return (finalPrices, usedCoupons);
        }

        private static Dictionary<long, double> ResolveInitialPrices(IThirdPartyPricesApi thirdPartyPricesApi, Dictionary<long, double> internalPrices, List<Product> products)
        {
            Dictionary<long, double> initialPrices = new();
            foreach (Product product in products)
            {
                double? price = internalPrices.ContainsKey(product.Id) ? internalPrices[product.Id] : null;
                if (price == null)
                {
                    price = thirdPartyPricesApi.FetchPrice(product.Id); // REST API call
                }
                initialPrices[product.Id] = (double)price; // TODO ask biz: what if not found in either internalPrices nor thirdPartyApi
            }

            return initialPrices;
        }
    }





    public interface ICouponRepo
    {
        void MarkUsedCoupons(long customerId, List<Coupon> usedCoupons);
    }
    public record Customer(List<Coupon> Coupons);
    public interface ICustomerRepo
    {
        Customer? FindById(long customerId);
        int CountByEmail(string email);
        long Save(Customer customer);
    }
    public class Coupon
    {
        private readonly ProductCategory? _category;
        private readonly int _discountAmount;
        private bool _autoApply = true;

        public Coupon(ProductCategory category, int discountAmount)
        {
            _category = category;
            _discountAmount = discountAmount;
        }

        public bool AutoApply => _autoApply;

        public void SetAutoApply(bool autoApply)
        {
            _autoApply = autoApply;
        }

        public bool IsApplicableFor(Product product)
        {
            return (product.Category == _category || _category == null) && !product.Premium;
        }

        public double Apply(Product product, double price)
        {
            if (!IsApplicableFor(product))
            {
                throw new ArgumentException();
            }
            return price - _discountAmount;
        }
    }
    public interface IThirdPartyPricesApi
    {
        double FetchPrice(long id);
    }
    public record OrderLine(Product Product, int ItemCount);
    public interface IProductRepo
    {
        List<long> GetHiddenProductIds();
        List<Product> FindAllById(List<long> productIds);
    }
    public enum ProductCategory
    {
        ELECTRONICS, KIDS, ME, HOME, UNCATEGORIZED
    }
    
    public class Product
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public ProductCategory Category { get; set; }
        public bool Premium { get; set; }
        public bool Deleted { get; set; }
    }
    public interface IOrderRepo
    {
        IEnumerable<Order> FindByActiveTrue();
    }
    public class Order
    {
        public long? Id { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
        public DateTime CreationDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public bool Active { get;  set; }
        public int Price { get; set; }
    }
}

