using System.Text.Json;
using MicroservicioDapper.Interfaces;
using MicroservicioDapper.Models;
using StackExchange.Redis;

namespace MicroservicioDapper.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string ProductIdsKey = "products:ids";
        private const string ProductIdSequenceKey = "products:next-id";

        private readonly IDatabase _database;

        public ProductRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var productIds = await _database.SetMembersAsync(ProductIdsKey);
            var productValues = await Task.WhenAll(productIds.Select(id =>
                _database.StringGetAsync(GetProductKey((int)id))));
            var products = new List<Product>(productValues.Length);

            foreach (var productValue in productValues)
            {
                products.Add(DeserializeProduct(productValue));
            }

            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var productValue = await _database.StringGetAsync(GetProductKey(id));
            return productValue.IsNull ? null : DeserializeProduct(productValue);
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            var id = checked((int)await _database.StringIncrementAsync(ProductIdSequenceKey));
            product.Id = id;
            product.CreatedDate = DateTime.UtcNow;

            var serializedProduct = JsonSerializer.Serialize(product);
            if (!await _database.StringSetAsync(GetProductKey(id), serializedProduct, when: When.NotExists))
            {
                throw new InvalidOperationException($"No se pudo guardar el producto con ID {id} en Redis.");
            }

            await _database.SetAddAsync(ProductIdsKey, id);
            return id;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var serializedProduct = JsonSerializer.Serialize(product);
            return await _database.StringSetAsync(
                GetProductKey(product.Id),
                serializedProduct,
                when: When.Exists);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var deleted = await _database.KeyDeleteAsync(GetProductKey(id));
            await _database.SetRemoveAsync(ProductIdsKey, id);
            return deleted;
        }

        private static string GetProductKey(int id) => $"products:{id}";

        private static Product DeserializeProduct(RedisValue value)
        {
            if (value.IsNull)
            {
                throw new InvalidOperationException("El índice de productos de Redis contiene un ID sin producto asociado.");
            }

            return JsonSerializer.Deserialize<Product>(value.ToString())
                ?? throw new InvalidOperationException("Redis devolvió un producto vacío o inválido.");
        }
    }
}