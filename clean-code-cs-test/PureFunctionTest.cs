using System;
using Moq;
using NUnit.Framework;
using Victor.Training.Cleancode;

namespace Victor.Training.Cleancode
{
    public class PureFunctionsTest
    {
        //private Mock<ICustomerRepo> customerRepo;
        //private Mock<IThirdPartyPricesApi> thirdPartyPrices;
        //private Mock<ICouponRepo> couponRepo;
        //private Mock<IProductRepo> productRepo;
        //private PureFunction priceService;

        //[SetUp]
        //public void Setup()
        //{
        //    customerRepo = new Mock<ICustomerRepo>();
        //    thirdPartyPrices = new Mock<IThirdPartyPricesApi>();
        //    couponRepo = new Mock<ICouponRepo>();
        //    productRepo = new Mock<IProductRepo>();
        //    priceService = new PureFunction(customerRepo.Object, thirdPartyPrices.Object, couponRepo.Object, productRepo.Object);
        //}
        //// TODO coupon.AutoApply
        //// TODO test not reusing coupons
        //[Test]
        //public void ComputePrices()
        //{
        //    var coupon1 = new Coupon(ProductCategory.HOME, 2);
        //    var coupon2 = new Coupon(ProductCategory.ELECTRONICS, 4);
        //    var customer = new Customer(new List<Coupon> { coupon1, coupon2 });
        //    customerRepo.Setup(repo => repo.FindById(13L)).Returns(customer);
        //    var p1 = new Product { Id = 1L, Category = ProductCategory.HOME };
        //    var p2 = new Product { Id = 2L, Category = ProductCategory.KIDS };
        //    productRepo.Setup(repo => repo.FindAllById(new List<long> { 1L, 2L })).Returns(new List<Product> { p1, p2 });
        //    thirdPartyPrices.Setup(api => api.FetchPrice(2L)).Returns(5d);

        //    var result = priceService.ComputePrices(13L, new List<long> { 1L, 2L }, new Dictionary<long, double> { { 1L, 10d } });

        //    couponRepo.Verify(repo => repo.MarkUsedCoupons(13L, It.Is<List<Coupon>>(list => list.Contains(coupon1) && !list.Contains(coupon2))));
        //    Assert.That(result, Has.Exactly(1).EqualTo(new KeyValuePair<long, double>(1L, 8d)));
        //    Assert.That(result, Has.Exactly(1).EqualTo(new KeyValuePair<long, double>(2L, 5d)));
        //}
    
        private Mock<ICustomerRepo> _customerRepoMock;
        private Mock<IThirdPartyPricesApi> _thirdPartyPricesApiMock;
        private Mock<ICouponRepo> _couponRepoMock;
        private Mock<IProductRepo> _productRepoMock;
        private PureFunction _pureFunction;

        [SetUp]
        public void SetUp()
        {
            _customerRepoMock = new Mock<ICustomerRepo>();
            _thirdPartyPricesApiMock = new Mock<IThirdPartyPricesApi>();
            _couponRepoMock = new Mock<ICouponRepo>();
            _productRepoMock = new Mock<IProductRepo>();

            _pureFunction = new PureFunction(
                _customerRepoMock.Object,
                _thirdPartyPricesApiMock.Object,
                _couponRepoMock.Object,
                _productRepoMock.Object
            );
        }

        [Test]
        public void ComputePrices_UsesInternalPriceIfAvailable()
        {
            // Arrange
            long customerId = 1;
            var productId = 101L;
            var product = new Product { Id = productId, Category = ProductCategory.ELECTRONICS, Premium = false };
            var internalPrices = new Dictionary<long, double> { { productId, 100.0 } };

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon>()));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product });

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId }, internalPrices);

            // Assert
            Assert.That(result[productId], Is.EqualTo(100.0));
            _thirdPartyPricesApiMock.Verify(api => api.FetchPrice(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void ComputePrices_FetchesPriceFromApiIfNoInternalPrice()
        {
            // Arrange
            long customerId = 1;
            var productId = 102L;
            var product = new Product { Id = productId, Category = ProductCategory.ELECTRONICS, Premium = false };
            var internalPrices = new Dictionary<long, double>(); // No internal price

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon>()));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product });

            _thirdPartyPricesApiMock.Setup(api => api.FetchPrice(productId)).Returns(120.0);

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId }, internalPrices);

            // Assert
            Assert.That(result[productId], Is.EqualTo(120.0));
            _thirdPartyPricesApiMock.Verify(api => api.FetchPrice(productId), Times.Once);
        }

        [Test]
        public void ComputePrices_AppliesAutoApplicableCoupons()
        {
            // Arrange
            long customerId = 1;
            var productId = 103L;
            var product = new Product { Id = productId, Category = ProductCategory.ELECTRONICS, Premium = false };
            var coupon = new Coupon(ProductCategory.ELECTRONICS, 20); // $20 discount
            var internalPrices = new Dictionary<long, double> { { productId, 150.0 } };

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon> { coupon }));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product });

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId }, internalPrices);

            // Assert
            Assert.That(result[productId], Is.EqualTo(130.0)); // 150 - 20
        }

        [Test]
        public void ComputePrices_EnsuresCouponsAreNotAppliedMoreThanOnce()
        {
            // Arrange
            long customerId = 1;
            var productId1 = 201L;
            var productId2 = 202L;
            var product1 = new Product { Id = productId1, Category = ProductCategory.ME, Premium = false };
            var product2 = new Product { Id = productId2, Category = ProductCategory.ME, Premium = false };
            var coupon = new Coupon(ProductCategory.ME, 30); // $30 discount

            var internalPrices = new Dictionary<long, double>
            {
                { productId1, 200.0 },
                { productId2, 220.0 }
            };

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon> { coupon }));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product1, product2 });

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId1, productId2 }, internalPrices);

            // Assert
            Assert.That(result[productId1], Is.EqualTo(170.0)); // 200 - 30
            Assert.That(result[productId2], Is.EqualTo(220.0)); // No discount, since coupon is used
        }


        [Test]
        public void ComputePrices_OnlyAppliesAutoApplyCoupons()
        {
            // Arrange
            long customerId = 1;
            var productId = 401L;
            var product = new Product { Id = productId, Category = ProductCategory.KIDS, Premium = false };

            var autoApplyCoupon = new Coupon(ProductCategory.KIDS, 15); // $15 discount
            var manualCoupon = new Coupon(ProductCategory.KIDS, 50); // $50 discount, but not AutoApply
            manualCoupon.SetAutoApply(false); // Explicitly disable auto-apply

            var internalPrices = new Dictionary<long, double> { { productId, 100.0 } };

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon> { autoApplyCoupon, manualCoupon }));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product });

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId }, internalPrices);

            // Assert
            Assert.That(result[productId], Is.EqualTo(85.0)); // 100 - 15 (Only AutoApply coupon is used)
        }
        [Test]
        public void ComputePrices_MarksUsedCoupons()
        {
            // Arrange
            long customerId = 1;
            var productId = 301L;
            var product = new Product { Id = productId, Category = ProductCategory.HOME, Premium = false };
            var coupon = new Coupon(ProductCategory.HOME, 25); // $25 discount
            var internalPrices = new Dictionary<long, double> { { productId, 180.0 } };

            _customerRepoMock.Setup(repo => repo.FindById(customerId))
                .Returns(new Customer(new List<Coupon> { coupon }));

            _productRepoMock.Setup(repo => repo.FindAllById(It.IsAny<List<long>>()))
                .Returns(new List<Product> { product });

            // Act
            var result = _pureFunction.ComputePrices(customerId, new List<long> { productId }, internalPrices);

            // Assert
            Assert.That(result[productId], Is.EqualTo(155.0)); // 180 - 25
            _couponRepoMock.Verify(repo => repo.MarkUsedCoupons(customerId, It.IsAny<List<Coupon>>()), Times.Once);
        }
    }
}

