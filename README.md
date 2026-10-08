# MicroservicioDapperRedis

###
>- Redis-8.8.0-Windows-x64-cygwin
>- redis-server
>- refis-cli
>- set name Ejemplo
>- get name

## Redis

Redis is the primary store for products. Configure its address in
`MicroservicioDapper/appsettings.json` under `ConnectionStrings:Redis` and make
sure the Redis server is running before starting the API.

Products are stored as JSON at `products:{id}`. The `products:ids` set indexes
product IDs, and `products:next-id` generates new IDs.

## API

The products endpoints are available under `/api/products`:

- `GET /api/products` lists products.
- `GET /api/products/{id}` retrieves a product.
- `POST /api/products` creates a product.
- `PUT /api/products/{id}` updates a product.
- `DELETE /api/products/{id}` deletes a product.

## Tests

Run the unit tests with:

```powershell
dotnet test .\MicroservicioDapper.Tests\MicroservicioDapper.Tests.csproj
```
