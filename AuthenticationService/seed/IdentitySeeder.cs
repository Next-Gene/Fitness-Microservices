using Microsoft.AspNetCore.Identity;
using AuthenticationService.Models;

namespace AuthenticationService.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedIdentityAsync(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            // Seed Admin
            var adminEmail = "seifmoataz27249@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            var adminUserId = Guid.Parse("11111111-2222-3333-4444-555555555555");

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    Id = adminUserId,
                    UserName = "SeifAdmin",
                    Email = adminEmail,
                    FirstName = "Seif",
                    LastName = "Moataz",
                    ActivtyLevel = "Medium",
                    Goal = "Lose Weight",
                    Age = 21,
                    Height = 180,
                    Weight = 120,
                    Gender = "Male",
                    EmailConfirmed = true,
                    PhoneNumber = "01000000000"
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded) await userManager.AddToRoleAsync(user, "Admin");
            }

            // Seed Mock Users
            var mockUsers = new List<ApplicationUser>
            {
                new() { UserName = "JohnWeightLoss", Email = "john@example.com", FirstName = "John", LastName = "Doe", Age = 30, Height = 175, Weight = 95, Gender = "Male", ActivtyLevel = "Low", Goal = "Lose Weight", EmailConfirmed = true },
                new() { UserName = "JaneMuscleGain", Email = "jane@example.com", FirstName = "Jane", LastName = "Smith", Age = 25, Height = 165, Weight = 55, Gender = "Female", ActivtyLevel = "High", Goal = "Gain Weight", EmailConfirmed = true },
                new() { UserName = "MikeFit", Email = "mike@example.com", FirstName = "Mike", LastName = "Johnson", Age = 35, Height = 185, Weight = 85, Gender = "Male", ActivtyLevel = "Medium", Goal = "Get Fitter", EmailConfirmed = true }
            };

            foreach (var user in mockUsers)
            {
                if (await userManager.FindByEmailAsync(user.Email!) == null)
                {
                    var result = await userManager.CreateAsync(user, "User@123");
                    if (result.Succeeded) await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
