using ProductCatalog.Core.Entities;

namespace ProductCatalog.DataAccess.Persistence;

public static class DbInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product()
                {
                    Code = "1234-5678",
                    Name = "Laptop",
                    Description = "A high-performance laptop for work.",
                    Price = 1200.00m
                },
                new Product()
                {
                    Code = "9876-5432",
                    Name = "Smartphone",
                    Description = "A sleek and powerful smartphone with excellent camera quality.",
                    Price = 800.00m
                },
                new Product()
                {
                    Code = "2468-1357",
                    Name = "Headphones",
                    Price = 150.00m
                }
            );

            context.SaveChanges();
        }
    }
}