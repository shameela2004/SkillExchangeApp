using MyApp1.Domain.Entities;
using MyApp1.Infrastructure.Data;
using MyApp1.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Seeders
{

    public static class DataSeeder
    {
        public static async Task SeedAdminUser(MyApp1DbContext context)
        {
            Console.WriteLine("Running Admin Seeder...");
            const string adminEmail = "admin@SkillLink.com";

            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                var adminUser = new User
                {
                    Name = "Super Admin",
                    Email = adminEmail,
                    PasswordHash = PasswordHasher.HashPassword("admin@SkillLink.com"), // Replace with your hash method
                    Role = "Admin",
                    MentorStatus = null ,
                    IsEmailVerified = true
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
