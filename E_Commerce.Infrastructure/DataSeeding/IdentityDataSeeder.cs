using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;

        public IdentityDataSeeder(StoreDbContext dbContext,
               UserManager<ApplicationUser> userManager,
               RoleManager<IdentityRole> roleManager,
               ILogger<IdentityDataSeeder> logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task SeedDataAsAsync(CancellationToken ct = default)
        {
            try
            {
                bool hasUser = await userManager.Users.AnyAsync(ct);
                bool hasRoles = await roleManager.Roles.AnyAsync(ct);

                if (hasUser && hasRoles) return;

                var roles = new List<IdentityRole>()
                {
                    new IdentityRole("SuperAdmin"),
                    new IdentityRole("Admin")
                };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {role.Name} : {string.Join(";", roleResult.Errors.Select(x => x.Description))}");
                        }
                    }
                }

                if (!hasUser)
                {
                    var usersToSeed = new List<(ApplicationUser User, string Password, string Role)>
                    {
                        (
                            new ApplicationUser { DisplayName = "Moaazatef", UserName = "MoaazAtef", Email = "moaazatef2020@gmail.com", PhoneNumber = "01153997317" },
                            "P@ssw0rd",
                            "SuperAdmin"
                        ),
                        (
                            new ApplicationUser { DisplayName = "BelalAtef", UserName = "BelalAtef", Email = "Belalatef2020@gmail.com", PhoneNumber = "01153997319" },
                            "P@ssw0rd",
                            "Admin"
                        )
                    };

                    foreach (var item in usersToSeed)
                    {
                        var addUserResult = await userManager.CreateAsync(item.User, item.Password);

                        if (!addUserResult.Succeeded)
                        {
                            logger.LogError($"Failed To Add User {item.User.UserName} : {string.Join(";", addUserResult.Errors.Select(x => x.Description))}");
                            continue;
                        }

                        var addRoleResult = await userManager.AddToRoleAsync(item.User, item.Role);

                        if (!addRoleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Add Role To {item.User.UserName} : {string.Join(";", addRoleResult.Errors.Select(x => x.Description))}");
                        }
                    }

                    logger.LogInformation("Identity Data Seeded");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
            }
        }
    }
}