using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext DbContext ,ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsAsync(CancellationToken ct = default)
        {
			try
			{
				var PendingMigrations = await DbContext.Database.GetPendingMigrationsAsync(ct);
				if (PendingMigrations.Any())
					await DbContext.Database.MigrateAsync(ct);

				//Data Seed

				var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

				await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
				await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
				await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);
                await SeedIfEmptyAsync<DeliveryMethod, int>(seedRoot, "delivery.json", ct);

                var result = await DbContext.SaveChangesAsync(ct);
				if (result > 0)
					logger.LogInformation($"{result} Row Added");
				else
					logger.LogInformation("Database Already Seeded");
            }
            catch (Exception)
			{
				return;
			}
        }

		private async Task SeedIfEmptyAsync<T,Tkey>(string rootPath,string fileName,CancellationToken ct) where T : BaseEntity<Tkey>
		{
			if(await DbContext.Set<T>().AnyAsync())
			{
				logger.LogInformation("Already Have Data Seeded");
				return;
			}


			var filePath = Path.Combine(rootPath, fileName);

			if(!File.Exists(filePath)) 
			{
				logger.LogWarning($"File {fileName} Not Exists");
				return;
			}

			using var fileStreem = File.OpenRead(filePath);
			var options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};
			var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStreem ,options ,ct);
			if (items?.Any() ?? false)
				DbContext.Set<T>().AddRange(items);
        }
    }
    }

