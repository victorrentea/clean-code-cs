using System;
namespace Victor.Training.Cleancode
{
    public class PureFunction
    {
        private readonly ICustomerRepo customerRepo;
        private readonly IThirdPartyPricesApi thirdPartyPricesApi;
        private readonly ICouponRepo couponRepo;
        private readonly IProductRepo productRepo;

        public PureFunction(ICustomerRepo customerRepo, IThirdPartyPricesApi thirdPartyPricesApi, ICouponRepo couponRepo, IProductRepo productRepo)
        {
            this.customerRepo = customerRepo;
            this.thirdPartyPricesApi = thirdPartyPricesApi;
            this.couponRepo = couponRepo;
            this.productRepo = productRepo;
        }

        // TODO extract complexity into a pure function
        public Dictionary<long, double> ComputePrices(long customerId, List<long> productIds, Dictionary<long, double> internalPrices)
        {
            Customer customer = customerRepo.FindById(customerId);
            List<Product> products = productRepo.FindAllById(productIds);
            Dictionary<long, double> basePrices = ResolveBasePrices(internalPrices, products);
            var (finalPrices, usedCoupons) = ApplyCoupons(customer.Coupons, products, basePrices);
            couponRepo.MarkUsedCoupons(customerId, usedCoupons);
            return finalPrices;
        }
         
        // PURE FUNCTION
        static (Dictionary<long, double> finalPrices, List<Coupon> usedCoupons) ApplyCoupons(
            List<Coupon> coupons,
            List<Product> products,
            Dictionary<long, double> basePrices)
        {
            // apply coupons
            List<Coupon> usedCoupons = new();
            Dictionary<long, double> finalPrices = new();
            foreach (Product product in products)
            {
                double price = basePrices[product.Id];
                foreach (Coupon coupon in coupons)
                {
                    if (coupon.AutoApply && coupon.IsApplicableFor(product) && !usedCoupons.Contains(coupon))
                    {
                        price = coupon.Apply(product, price);
                        usedCoupons.Add(coupon);
                    }
                }
                finalPrices[product.Id] = price;
            }

            return (finalPrices, usedCoupons);
        }

        
        private Dictionary<long, double> ResolveBasePrices(Dictionary<long, double> internalPrices, List<Product> products)
        {
            //Dictionary<long, double> basePrices = new();
            //foreach (Product product in products)
            //{
            //    double price = internalPrices.ContainsKey(product.Id) ?
            //        internalPrices[product.Id] :
            //        thirdPartyPricesApi.FetchPrice(product.Id);
            //    basePrices.Add(product.Id, price);
            //}
            //return basePrices;
            return products.ToDictionary(
                 product => product.Id,
                 product => internalPrices.ContainsKey(product.Id)
                     ? internalPrices[product.Id]
                     : thirdPartyPricesApi.FetchPrice(product.Id)
             );
        }
    }
    public interface ICouponRepo
    {
        void MarkUsedCoupons(long customerId, List<Coupon> usedCoupons);
    }
    public record Customer(List<Coupon> Coupons);
    public interface ICustomerRepo
    {
        Customer FindById(long customerId);
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
        public bool Active { get; set; }
        public int Price { get; set; }
        public bool IsRecent { get => CreationDate > DateTime.Now.AddYears(-1); }
    }
}

