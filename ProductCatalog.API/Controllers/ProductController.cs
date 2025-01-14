using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Exceptions;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("/api")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    private readonly IValidator<ProductDto> _validator;
    private readonly IValidator<string> _codeValidator;

    public ProductController(
        IProductService productService,
        IValidator<ProductDto> validator,
        ILogger<ProductController> logger,
        IValidator<string> codeValidator)
    {
        _productService = productService;
        _validator = validator;
        _codeValidator  = codeValidator;
        _logger = logger;
    }
    
    /// <summary>
    /// Створення нового товару
    /// </summary>
    /// <returns></returns>
    [HttpPost("products")]
    [ProducesResponseType(type: typeof(List<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
    {
        try
        {
            var validationResult = _validator.Validate(productDto);
            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = validationResult.Errors });
            }

            await _productService.CreateProductAsync(productDto);
            
            _logger.LogInformation($"Product created with code {productDto.Code}");

            return Ok();
        }
        catch (EntityAlreadyExistsException ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Отримання списку товарів
    /// </summary>
    /// <returns></returns>
    [HttpGet("products")]
    [ProducesResponseType(type: typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetProductsAsync();
            
            _logger.LogInformation($"Retrieved {products.Count()} products");

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Отримання детальної інформації про товар по коду товару
    /// </summary>
    /// <returns></returns>
    [HttpGet("products/{code}")]
    [ProducesResponseType(type: typeof(List<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductByCode(string code)
    {
        try
        {
            var validationResult = _codeValidator.Validate(code);
            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = validationResult.Errors });
            }

            var product = await _productService.GetProductByCodeAsync(code);
            
            _logger.LogInformation($"Retrieved product with code {code}");

            return Ok(product);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Редагування товару
    /// </summary>
    /// <returns></returns>
    [HttpPut("products")]
    [ProducesResponseType(type: typeof(List<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
    {
        try
        {
            var validationResult = _validator.Validate(productDto);
            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = validationResult.Errors });
            }

            await _productService.UpdateProductAsync(productDto);
            
            _logger.LogInformation($"Product updated with code {productDto.Code}");

            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Видалення товару
    /// </summary>
    /// <returns></returns>
    [HttpDelete("products")]
    [ProducesResponseType(type: typeof(List<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct([FromBody] string productCode)
    {
        try
        {
            var validationResult = _codeValidator.Validate(productCode);
            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = validationResult.Errors });
            }

            await _productService.DeleteProductAsync(productCode);
            
            _logger.LogInformation($"Product deleted with code {productCode}");

            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest(ex.Message);
        }
    }
}