using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Api.Test.Support;
using Xunit;

namespace Shop.Catalog.Api.Test.Controllers
{
    public class ProductsControllerTest
    {
        public ProductsControllerTest()
        {
            var testfixture = new TestFixture<Startup>(services => { });

            _client = testfixture.Client;
        }

        private readonly HttpClient _client;

        [Fact(DisplayName = "Get should return all products")]
        public async Task ReturnsAllProducts()
        {
            var response = await _client.GetAsync("/v1/products");
            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Envelope<IEnumerable<object>>>(responseText);

            Assert.Equal(6, products.Data.Count());
        }
    }
}