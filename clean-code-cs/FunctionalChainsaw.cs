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
            List<long> hiddenIds = productRepo.GetHiddenProductIds();
            return orders
                .Where(order => order.Active)
                .Where(order => order.IsRecent)
                .SelectMany(order => order.OrderLines)
                .GroupBy(orderLine => orderLine.Product, orderLine => orderLine.ItemCount)
                .Select(g=> new { key=g.Key, sum=g.Sum()}) // SELECT key, SUM(g.itemCount
                //.ToDictionary(g => g.Key, g => g.Sum()) // PROBLEM
                .Where(entry => entry.sum >= 10)
                .Select(entry => entry.key)
                .Where(product => !product.Deleted)
                //.Join
                .Where(product => !hiddenIds.Contains(product.Id))
                .ToList();
        }

        
    }
}

