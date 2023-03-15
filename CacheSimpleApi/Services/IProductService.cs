namespace CacheSimpleApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
    }

}