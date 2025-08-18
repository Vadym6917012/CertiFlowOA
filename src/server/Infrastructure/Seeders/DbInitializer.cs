using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeders
{
    public static class DbInitializer
    {
        public static void Seed(DataContext ctx)
        {
            ctx.Database.EnsureCreated();

            if ( !ctx.Faculties.Any() )
            {
                ctx.Faculties.AddRange(
                    new Faculty { Name = "Навчально-науковий інститут соціально-гуманітарного менеджменту" },
                    new Faculty { Name = "Навчально-науковий інститут міжнародних відносин та національної безпеки" },
                    new Faculty { Name = "Навчально-науковий інститут права ім. І. Малиновського" },
                    new Faculty { Name = "Навчально-науковий інститут лінгвістики" },
                    new Faculty { Name = "Навчально-науковий інститут інформаційних технологій та бізнесу" },
                    new Faculty { Name = "Навчально-науковий центр заочно-дистанційного навчання" }
                );
                ctx.SaveChanges();
            }

            if ( !ctx.DocumentTypes.Any() )
            {
                ctx.DocumentTypes.AddRange(
                    new DocumentType { Name = "Довідка про навчання" },
                    new DocumentType { Name = "Виписка з академічної успішності" },
                    new DocumentType { Name = "Диплом" },
                    new DocumentType { Name = "Сертифікат" }
                );
                ctx.SaveChanges();
            }
        }
    }
}
