using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Exceptions;
using ProductCatalog.DataAccess.Persistence;
using ProductCatalog.DataAccess.Repositories;

namespace ProductCatalog.Application.Services.Impl;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly DatabaseContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(
        ILogger<ProductService> logger,
        DatabaseContext context,
        IMapper mapper, 
        IProductRepository productRepository)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task CreateProductAsync(ProductDto productDto)
    {
        var product = await _productRepository.GetProductByCodeAsync(productDto.Code);
        if (product is not null)
        {
            _logger.LogError($"Product with code {productDto.Code} already exists.");

            throw new EntityAlreadyExistsException();
        }
        
        await _context.Products.AddAsync(_mapper.Map<Product>(productDto));
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductListDto>> GetProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        
        return _mapper.Map<List<ProductListDto>>(products);
    }

    public async Task UpdateProductAsync(ProductDto updateProductDto)
    {
        var product = await _productRepository.GetProductByCodeAsync(updateProductDto.Code);
        if (product is null)
        {
            _logger.LogError("Product with Code {Code} not found", updateProductDto.Code);

            throw new EntityNotFoundException();
        }
        
        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Description = updateProductDto.Description;
        
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(string productCode)
    {
        var product = await _productRepository.GetProductByCodeAsync(productCode);
        if (product is null)
        {
            _logger.LogError("Product with Code {Code} not found", productCode);
            
            throw new EntityNotFoundException();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    
    public async Task<ProductDto> GetProductByCodeAsync(string productCode)
    {
        var product = await _productRepository.GetProductByCodeAsync(productCode);
        if (product is null)
        {
            _logger.LogError("Product with Code {Code} not found", productCode);
            
            throw new EntityNotFoundException();
        }
        
        var response = _mapper.Map<ProductDto>(product);
        
        return response;
    }
}