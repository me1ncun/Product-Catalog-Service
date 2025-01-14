using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataAccess.Persistence;

namespace ProductCatalogService.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        context.Database.Migrate();
    }
}