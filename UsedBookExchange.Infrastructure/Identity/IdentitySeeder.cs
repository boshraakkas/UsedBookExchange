using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsedBookExchange.Infrastructure.Identity;

namespace UsedBookExchange.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    {
        var roleManager =
        serviceProvider.GetRequiredService<
        RoleManager<IdentityRole>>();

    var userManager =
        serviceProvider.GetRequiredService<
            UserManager<ApplicationUser>>();

        const string adminRole = "Admin";

        // Create Admin role if it does not exist.
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            var roleResult =
                await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to create Admin role.");
            }
        }

        // Read admin credentials from configuration.
        var adminEmail =
            configuration["Admin:Email"];

        var adminPassword =
            configuration["Admin:Password"];

        // Do not create an admin user
        // if credentials are not configured.
        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "System Administrator"
            };

            var createResult =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(
                        error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to create admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
                adminUser,
                adminRole))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    adminUser,
                    adminRole);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to assign Admin role.");
            }
        }
    }


}
