using System;
namespace Victor.Training.Cleancode
{

    public class PureFunction
    {
        private readonly CustomerRepo customerRepo;
        private readonly ThirdPartyPricesApi thirdPartyPricesApi;
        private readonly CouponRepo couponRepo;
        private readonly ProductRepo productRepo;

        public PureFunction(CustomerRepo customerRepo, ThirdPartyPricesApi thirdPartyPricesApi, CouponRepo couponRepo, ProductRepo productRepo)
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

            List<Coupon> usedCoupons = new List<Coupon>();
            Dictionary<long, double> finalPrices = new Dictionary<long, double>();
            foreach (Product product in products)
            {
                double? price = internalPrices.ContainsKey(product.Id) ? internalPrices[product.Id] : (double?)null;
                if (price == null)
                {
                    price = thirdPartyPricesApi.FetchPrice(product.Id);
                }
                foreach (Coupon coupon in customer.Coupons)
                {
                    if (coupon.AutoApply && coupon.IsApplicableFor(product) && !usedCoupons.Contains(coupon))
                    {
                        price = coupon.Apply(product, price.Value);
                        usedCoupons.Add(coupon);
                    }
                }
                finalPrices[product.Id] = price.Value;
            }

            couponRepo.MarkUsedCoupons(customerId, usedCoupons);
            return finalPrices;
        }
    }
    public interface CouponRepo
    {
        void MarkUsedCoupons(long customerId, List<Coupon> usedCoupons);
    }
    public record Customer(List<Coupon> Coupons);
    public interface CustomerRepo
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
    public interface ThirdPartyPricesApi
    {
        double FetchPrice(long id);
    }
    public record OrderLine(Product Product, int ItemCount);
    public interface ProductRepo
    {
        List<long> GetHiddenProductIds();
        List<Product> FindAllById(List<long> productIds);
    }
    public enum ProductCategory
    {
        ELECTRONICS, KIDS, ME, HOME, UNCATEGORIZED
    }
    public interface OrderRepo
    {
        IEnumerable<Order> FindByActiveTrue();
    }
    public class Product
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public ProductCategory Category { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Premium { get; set; }
        public bool Deleted { get; set; }

        public Product(string name, ProductCategory category)
        {
            Name = name;
            Category = category;
        }

        public Product(string name)
        {
            Name = name;
        }

        public Product() { }
    }
    public class Order
    {
        public long? Id { get; private set; }
        public List<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();
        public DateTime CreationDate { get; private set; }
        public DateTime? ShipDate { get; private set; }
        public bool Active { get; private set; }
        public int Price { get; private set; }

        public Order SetId(long? id)
        {
            Id = id;
            return this;
        }

        public Order SetOrderLines(List<OrderLine> orderLines)
        {
            OrderLines = orderLines;
            return this;
        }

        public Order SetCreationDate(DateTime creationDate)
        {
            CreationDate = creationDate;
            return this;
        }

        public Order SetShipDate(DateTime? shipDate)
        {
            ShipDate = shipDate;
            return this;
        }

        public Order SetActive(bool active)
        {
            Active = active;
            return this;
        }

        public Order SetPrice(int price)
        {
            Price = price;
            return this;
        }
    }
}

