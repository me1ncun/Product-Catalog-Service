using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Services;

public interface IProductService
{
    Task CreateProductAsync(ProductDto productDto);
    Task<PagedList<ProductListDto>> GetProductsAsync(GetProductsDto request);
    Task UpdateProductAsync(ProductDto updateProductDto);
    Task DeleteProductAsync(string productCode);
    Task<ProductDto> GetProductByCodeAsync(string productCode);
}