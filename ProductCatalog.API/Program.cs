using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.MappingProfiles;
using ProductCatalog.Application.Services;
using ProductCatalog.Application.Services.Impl;
using ProductCatalog.Application.Validators;
using ProductCatalog.DataAccess.Persistence;
using ProductCatalog.DataAccess.Repositories;
using ProductCatalog.DataAccess.Repositories.Impl;
using ProductCatalogService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering Database
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
});

// Registering other dependencies
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

// Fluent Validator
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateProductValidator));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.CreateDbIfNotExists();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();