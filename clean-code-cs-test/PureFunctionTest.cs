using System;
using Moq;
using NUnit.Framework;
using Victor.Training.Cleancode;

namespace Victor.Training.Cleancode
{
    public class PureFunctionsTest
    {
        private Mock<CustomerRepo> customerRepo;
        private Mock<ThirdPartyPricesApi> thirdPartyPrices;
        private Mock<CouponRepo> couponRepo;
        private Mock<ProductRepo> productRepo;
        private PureFunction priceService;

        [SetUp]
        public void Setup()
        {
            customerRepo = new Mock<CustomerRepo>();
            thirdPartyPrices = new Mock<ThirdPartyPricesApi>();
            couponRepo = new Mock<CouponRepo>();
            productRepo = new Mock<ProductRepo>();
            priceService = new PureFunction(customerRepo.Object, thirdPartyPrices.Object, couponRepo.Object, productRepo.Object);
        }

        [Test]
        public void ComputePrices()
        {
            var coupon1 = new Coupon(ProductCategory.HOME, 2);
            var coupon2 = new Coupon(ProductCategory.ELECTRONICS, 4);
            var customer = new Customer(new List<Coupon> { coupon1, coupon2 });
            customerRepo.Setup(repo => repo.FindById(13L)).Returns(customer);
            var p1 = new Product { Id = 1L, Category = ProductCategory.HOME };
            var p2 = new Product { Id = 2L, Category = ProductCategory.KIDS };
            productRepo.Setup(repo => repo.FindAllById(new List<long> { 1L, 2L })).Returns(new List<Product> { p1, p2 });
            thirdPartyPrices.Setup(api => api.FetchPrice(2L)).Returns(5d);

            var result = priceService.ComputePrices(13L, new List<long> { 1L, 2L }, new Dictionary<long, double> { { 1L, 10d } });

            couponRepo.Verify(repo => repo.MarkUsedCoupons(13L, It.Is<List<Coupon>>(list => list.Contains(coupon1) && !list.Contains(coupon2))));
            Assert.That(result, Has.Exactly(1).EqualTo(new KeyValuePair<long, double>(1L, 8d)));
            Assert.That(result, Has.Exactly(1).EqualTo(new KeyValuePair<long, double>(2L, 5d)));
        }
    }
}

