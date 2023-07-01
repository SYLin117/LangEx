using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using ErrorProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ErrorProject.Data
{
    public class Seed
    {
        public static async Task ClearConnections(DataContext context)
        {
            context.Connections.RemoveRange(context.Connections); // remove rows in connection table
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, DataContext context)
        {
            // only seed user when no data in database
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/Users.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Member" },
                new AppRole { Name = "Admin" },
                new AppRole { Name = "Moderator" },
            };


            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }


            int langCount = context.Languages.Count(); // Get the total number of rows in the table
            int countryCount = context.Countries.Count();
            foreach (var user in users)
            {
                Random random = new Random();
                int randIndex1 = random.Next(0, langCount);
                int randIndex2 = random.Next(0, langCount);
                int randCountryIndex = random.Next(0, countryCount);
                int desiredRowCount = random.Next(1, 5);
                var langRows1 = context.Languages
                    .Skip(randIndex1)
                    .Take(desiredRowCount)
                    .ToList();
                var langRows2 = context.Languages
                    .Skip(randIndex2)
                    .Take(desiredRowCount)
                    .ToList();
                var country = context.Countries
                    .Skip(randCountryIndex)
                    .Take(1)
                    .FirstOrDefault();
                user.UserName = user.UserName.ToLower();
                // specify utc datetime or it will throw error on postgresSQL
                user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
                user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
                user.Languages = langRows1;
                user.InterestedLanguages = langRows2;
                user.CountryId = country.Id;

                await userManager.CreateAsync(user, "P@ssw0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "P@ssw0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }

        public static async Task SeedStatic(DataContext context)
        {
            await context.Languages.ExecuteDeleteAsync();
            await context.Countries.ExecuteDeleteAsync();

            var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using (var reader = new StreamReader("Data/Language.csv"))
            using (var csv = new CsvReader(reader, conf))
            {
                // Read the CSV records
                var records = csv.GetRecords<Language>().ToList();
                foreach (var record in records)
                {
                    // whatever u want
                    await context.Languages.AddAsync(record);
                }
            }

            using (var reader = new StreamReader("Data/Country.csv"))
            using (var csv = new CsvReader(reader, conf))
            {
                // Read the CSV records
                var records = csv.GetRecords<Country>().ToList();
                int idx = 1;
                foreach (var record in records)
                {
                    // whatever u want
                    Country newCountry = new Country()
                    {
                        Id = idx,
                        Name = record.Name,
                        Code = record.Code
                    };

                    await context.Countries.AddAsync(newCountry);
                    idx++;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}