using E_Commerce.Domain.Contracts;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scop = app.Services.CreateScope();
            var seeder = scop.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            var identitySeeder = scop.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");

            await seeder.SeedDataAsAsync();
            await identitySeeder.SeedDataAsAsync();
            return app;
        }
}
}
