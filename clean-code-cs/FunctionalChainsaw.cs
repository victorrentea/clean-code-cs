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
            return orders
                .Where(order => order.Active)
                .Where(order => order.CreationDate > DateTime.Now.AddYears(-1))
                .SelectMany(order => order.OrderLines)
                .GroupBy(orderLine => orderLine.Product, orderLine => orderLine.ItemCount)
                .ToDictionary(g => g.Key, g => g.Sum())
                .Where(entry => entry.Value >= 10)
                .Select(entry => entry.Key)
                .Where(product => !product.Deleted)
                .Where(product => !productRepo.GetHiddenProductIds().Contains(product.Id))
                .ToList();
        }
    }
}

