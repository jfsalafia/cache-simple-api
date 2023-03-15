using Microsoft.AspNetCore.Mvc;
using CacheSimpleApi.Services;
using ZiggyCreatures.Caching.Fusion;

namespace CacheSimpleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFusionCache _cache;
        private const string ProductCacheKey = "PRODUCTS_CACHE";

        public ProductController(IProductService productService, IFusionCache cache)
        {
            _productService = productService;
            _cache = cache;    
        }

        [HttpGet("cached")]
        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            var items = await _cache.GetOrSetAsync(ProductCacheKey, _ => _productService.GetProductsAsync(cancellationToken), TimeSpan.FromMinutes(1), cancellationToken);
            return (items ?? Enumerable.Empty<Product>());
        }
    }
}
