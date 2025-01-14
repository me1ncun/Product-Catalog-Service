using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Exceptions;
using ProductCatalogService.Controllers;

namespace ProductCatalog.API.Tests;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IValidator<ProductDto>> _mockValidator;
    private readonly Mock<IValidator<string>> _mockCodeValidator;
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockValidator = new Mock<IValidator<ProductDto>>();
        _mockCodeValidator = new Mock<IValidator<string>>();
        _mockLogger = new Mock<ILogger<ProductController>>();

        _controller = new ProductController(
            _mockProductService.Object,
            _mockValidator.Object,
            _mockLogger.Object,
            _mockCodeValidator.Object
        );
    }

    [Fact]
    public async Task AddProduct_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var productDto = new ProductDto { Code = "ABC123" };
        var validationFailures = new List<ValidationFailure> { new("Code", "Invalid code format") };
        _mockValidator
            .Setup(v => v.Validate(productDto))
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _controller.AddProduct(productDto);

        // Assert
        var badRequestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnOk_WhenValidProductIsAdded()
    {
        // Arrange
        var productDto = new ProductDto { Code = "1234-5678" };
        _mockValidator
            .Setup(v => v.Validate(productDto))
            .Returns(new ValidationResult());

        _mockProductService
            .Setup(s => s.CreateProductAsync(productDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddProduct(productDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnProducts()
    {
        // Arrange
        var products = new List<ProductListDto> { new ProductListDto
        {
            Code = "1234-5678",
            Name = "Laptop",
            Price = 10.00M
        }};
        
        var request = new GetProductsDto
        {
            SearchItem = "Laptop",
            SortOrder = "asc",
            SortColumn = "code",
            Page = 1,
            PageSize = 10
        };

        var mockQueryable = products.AsQueryable().BuildMockDbSet();
        
        var pagedList = await PagedList<ProductListDto>.CreateAsync(mockQueryable.Object, request.Page, request.PageSize);
        
        _mockProductService
            .Setup(s => s.GetProductsAsync(It.IsAny<GetProductsDto>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _controller.GetAllProducts(
            request.SearchItem,
            request.SortColumn,
            request.SortOrder,
            request.Page,
            request.PageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsAssignableFrom<PagedList<ProductListDto>>(okResult.Value);
        Assert.Single(returnedProducts.Items);
    }

    [Fact]
    public async Task GetProductByCode_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var code = "ABC123";
        var validationFailures = new List<ValidationFailure> { new("Code", "Invalid code format") };
        _mockCodeValidator
            .Setup(v => v.Validate(code))
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _controller.GetProductByCode(code);

        // Assert
        var badRequestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var code = "1111-2222";
        _mockCodeValidator
            .Setup(v => v.Validate(code))
            .Returns(new ValidationResult());

        _mockProductService
            .Setup(s => s.DeleteProductAsync(code))
            .ThrowsAsync(new EntityNotFoundException());

        // Act
        var result = await _controller.DeleteProduct(code);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}