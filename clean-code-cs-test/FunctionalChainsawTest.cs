using NUnit.Framework;
using Moq;

namespace Victor.Training.Cleancode
{
    public class FunctionalChainsawTests
    {
        private Mock<IProductRepo> productRepo;
        private FunctionalChainsaw functionalChainsaw;

        [SetUp]
        public void Setup()
        {
            productRepo = new Mock<IProductRepo>();
            functionalChainsaw = new FunctionalChainsaw(productRepo.Object);
        }

        [Test]
        public void GetFrequentOrderedProducts_ReturnsProductsOrderedMoreThan10Times()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = true,
                    CreationDate = DateTime.Now.AddMonths(-6),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = false }, 10),
                        new OrderLine(new Product { Id = 1, Deleted = false }, 6)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long>());

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result, Has.Exactly(1).Matches<Product>(p => p.Id == 1));
        }

        [Test]
        public void GetFrequentOrderedProducts_ExcludesOldOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = true,
                    CreationDate = DateTime.Now.AddMonths(-13),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = false }, 11)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long>());

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result,Is.Empty);
        }

        [Test]
        public void GetFrequentOrderedProducts_ExcludesDeletedProducts()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = true,
                    CreationDate = DateTime.Now.AddMonths(-6),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = true }, 11)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long>());

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetFrequentOrderedProducts_ExcludesHiddenProducts()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = true,
                    CreationDate = DateTime.Now.AddMonths(-6),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = false }, 11)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long> { 1 });

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetFrequentOrderedProducts_ExcludesInactiveOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = false,
                    CreationDate = DateTime.Now.AddMonths(-6),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = false }, 11)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long>());

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetFrequentOrderedProducts_ExcludesOldOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Active = true,
                    CreationDate = DateTime.Now.AddYears(-2),
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine(new Product { Id = 1, Deleted = false }, 11)
                    }
                }
            };
            productRepo.Setup(repo => repo.GetHiddenProductIds()).Returns(new List<long>());

            var result = functionalChainsaw.GetFrequentOrderedProducts(orders);

            Assert.That(result, Is.Empty);
        }
    }
}