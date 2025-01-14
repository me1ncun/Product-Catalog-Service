using ProductCatalog.Core.Entities;

namespace ProductCatalog.DataAccess.Repositories;

public interface IProductRepository
{
    Task<Product> GetProductByCodeAsync(string productCode);
}