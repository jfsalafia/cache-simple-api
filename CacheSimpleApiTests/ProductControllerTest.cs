using CacheSimpleApi.Controllers;
using FluentAssertions;
using CacheSimpleApi.Services;
using Microsoft.Extensions.Options;
using Moq;
using ZiggyCreatures.Caching.Fusion;

namespace CacheSimpleApiTests
{
    public class ProductControllerTest
    {
        private Mock<IProductService> ProductService { get; }
        private IFusionCache FusionCache { get; }
        private IOptions<FusionCacheOptions> optionsAccessor { get; }

        private readonly ProductController Sut;
        public ProductControllerTest()
        {
            this.optionsAccessor = Options.Create(new FusionCacheOptions() {
                DefaultEntryOptions = new FusionCacheEntryOptions()
                {
                    Duration = TimeSpan.FromMinutes(1),
                    Priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low
                }
            });

            this.ProductService = new Mock<IProductService>();

            var products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Description = "Test"
                },
                new Product()
                {
                    Id = 2,
                    Description = "Test-2"
                }
            };

            this.ProductService.Setup(service => service.GetProductsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);
            
            this.FusionCache = new FusionCache(optionsAccessor);

            this.Sut = new ProductController(this.ProductService.Object, this.FusionCache);
        }

        [Fact]
        public async Task Should_Get_Products_From_External_Source()
        {
            //arrange
            var token = System.Threading.CancellationToken.None;

            //act
            var products = await this.Sut.GetProductsAsync(token);

            //assert
            products.Should().NotBeNullOrEmpty();
        }
    }
}