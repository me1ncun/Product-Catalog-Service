using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.DataAccess.Persistence;

namespace ProductCatalog.DataAccess.Repositories.Impl;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _databaseContext;

    public ProductRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Product> GetProductByCodeAsync(string productCode)
    {
        return await _databaseContext.Products
            .Where(x => x.Code == productCode)
            .FirstOrDefaultAsync();
    }
}