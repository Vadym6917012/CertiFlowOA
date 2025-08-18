using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            await SeedRolesAsync(roleManager);

            SeedFaculties(context);
            SeedDocumentType(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { SD.StudentRole, SD.SystemAdminRole, SD.FacultyAdminRole, SD.FacultyCuratorRole };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }

        private static void SeedFaculties(DataContext context)
        {
            if (!context.Faculties.Any())
            {
                context.Faculties.AddRange(new Faculty
                {
                    //FacultyId = 1,
                    Name = "Навчально-науковий інститут соціально-гуманітарного менеджменту",
                },
            new Faculty
            {
                //FacultyId = 2,
                Name = "Навчально-науковий інститут міжнародних відносин та національної безпеки",
            },
            new Faculty
            {
                //FacultyId = 3,
                Name = "Навчально-науковий інститут права ім. І. Малиновського",
            },
            new Faculty
            {
                //FacultyId = 4,
                Name = "Навчально-науковий інститут лінгвістики",
            },
            new Faculty
            {
                //FacultyId = 5,
                Name = "Економічний факультет",
            },
            new Faculty
            {
                //FacultyId = 6,
                Name = "Навчально-науковий центр заочно-дистанційного навчання",
            });

                context.SaveChanges();
            }
        }

        private static void SeedDocumentType(DataContext context)
        {
            if (!context.DocumentTypes.Any())
            {
                context.AddRange(
                new DocumentType
                {
                    Name = "Академічна довідка"
                }, new DocumentType
                {
                    Name = "Довідка про навчання"
                }, new DocumentType
                {
                    Name = "Довідка для військкомату"
                }, new DocumentType
                {
                    Name = "Довідка про доходи"
                }, new DocumentType
                {
                    Name = "Довідка для банку"
                }, new DocumentType
                {
                    Name = "Довідка про академічну успішність"
                }, new DocumentType
                {
                    Name = "Довідка про проходження практики"
                }, new DocumentType
                {
                    Name = "Довідка про відрахування або поновлення"
                });

                context.SaveChanges();
            }
        }
    }
}
