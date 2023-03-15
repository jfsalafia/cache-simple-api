using Newtonsoft.Json;

namespace CacheSimpleApi.Services
{
    public class ProductService : IProductService
    {
        public string ProductApiURL = "https://dummyjson.com/products";

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                // Send a GET request to the API
                HttpResponseMessage response = await client.GetAsync(ProductApiURL);
                Console.WriteLine($"{DateTime.Now} - Products Not Found in Cache! Returning data from external Products API");
                // Check the status code of the response
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var data = JsonConvert.DeserializeObject<ProductResponse>(responseString);
                        return data?.Products ??  Enumerable.Empty<Product>();
                    }
                }
            }
            return Enumerable.Empty<Product>();
        }
    }
}
