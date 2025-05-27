using System;
namespace Victor.Training.Cleancode
{
    public class FunctionalChainsaw
    {
        private readonly IProductRepo productRepo;

        public FunctionalChainsaw(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }

        public List<Product> GetFrequentOrderedProducts(List<Order> orders)
        {
            List<long> hiddenProductIds = productRepo.GetHiddenProductIds();
            IEnumerable<Product> frequentProducts = OrderedProductCounts(orders)
                            .Where(entry => entry.Value >= 10)
                            .Select(entry => entry.Key);
            
            return frequentProducts
                .Where(product => !product.Deleted)
                .Where(product => !hiddenProductIds.Contains(product.Id))
                .ToList();
        }

        private static Dictionary<Product, int> OrderedProductCounts(List<Order> orders)
        {
            return orders
                            .Where(order => order.Active)
                            .Where(order => order.CreationDate > DateTime.Now.AddYears(-1))
                            .SelectMany(order => order.OrderLines)
                            .GroupBy(orderLine => orderLine.Product, orderLine => orderLine.ItemCount)
                            .ToDictionary(g => g.Key, g => g.Sum());
        }
    }
}

