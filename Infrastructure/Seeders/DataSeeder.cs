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
            if (!context.Skills.Any())
            {
                var skills = new List<Skill>
            {
                new Skill { Name = "DotNet" },
                new Skill { Name = "JavaScript" },
                new Skill { Name = "C#" },
                new Skill { Name = "SQL" },
                new Skill { Name = "Communication" },
                new Skill { Name = "Guitar" },
                new Skill { Name = "Piano" },
                new Skill { Name = "Singing" },
                new Skill { Name = "Painting" },
                new Skill { Name = "Photography" },
                new Skill { Name = "Yoga" },
                new Skill { Name = "Graphic Design" },
            };

                context.Skills.AddRange(skills);
            }

            if (!context.Languages.Any())
            {
                var languages = new List<Language>
            {
                new Language { Code = "en", Name = "English" },
                new Language { Code = "ml", Name = "Malayalam" },
                new Language { Code = "fr", Name = "French" },
                new Language { Code = "es", Name = "Spanish" },
                new Language { Code = "de", Name = "German" },
                new Language { Code = "zh", Name = "Chinese" },
                new Language { Code = "hi", Name = "Hindi" },
                new Language { Code = "ar", Name = "Arabic" },
                new Language { Code = "ru", Name = "Russian" },
                new Language { Code = "ja", Name = "Japanese" },
                new Language { Code = "pt", Name = "Portuguese" },
                new Language { Code = "bn", Name = "Bengali" },
                new Language { Code = "ur", Name = "Urdu" },
                new Language { Code = "ko", Name = "Korean" },
                new Language { Code = "it", Name = "Italian" },
                new Language { Code = "nl", Name = "Dutch" }
            };

                context.Languages.AddRange(languages);
            }
            context.SaveChanges();
        }
    }
}
