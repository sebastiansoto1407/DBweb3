using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bogus;
using crudsweb3.data;
using crudsweb3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// Este seeder genera 50 tareas de prueba usando la libreria Bogus
//asegura que nombreTarea tenga al menos 3 letras, sin simbolos, y que fecha/hora sean futuras
// Puede ejecutarse con reset=true para borrar datos anteriores antes de sembrar
namespace crudsweb3.Seeders
{
    public class TareaSeeder : IDbInitializer
    {
        public void Initialize(IServiceProvider serviceProvider, bool reset = false)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TareaDbContext>();
            context.Database.Migrate();

            SeedTareaData(context, reset);
        }

        private static void SeedTareaData(TareaDbContext context, bool reset)
        {
            if (reset)
            {
                context.Tareas.RemoveRange(context.Tareas);
                context.SaveChanges();
            }
            else
            {
                if (context.Tareas.AsNoTracking().Any()) return;
            }

            var estados = new[] { "Pendiente", "En Curso", "Completo", "Inactivo" };

            var faker = new Faker<Tarea>("es")
                .RuleFor(t => t.nombreTarea, f =>
                {
                    var raw = string.Join(" ", f.Lorem.Words(f.Random.Int(2, 4)));
                    var soloLetras = Regex.Replace(raw, @"[^\p{L}\s]", " ");
                    var normal = Regex.Replace(soloLetras, @"\s+", " ").Trim();
                    if (Regex.Matches(normal, @"\p{L}").Count < 3)
                        normal += " tarea";
                    return char.ToUpper(normal[0]) + normal.Substring(1);
                })
                .RuleFor(t => t.fechaVencimiento, f =>
                    DateTime.SpecifyKind(f.Date.Future(1), DateTimeKind.Local))
                .RuleFor(t => t.estado, f => f.PickRandom(estados))
                .RuleFor(t => t.IdUsuario, f => f.Random.Int(1, 10));

            var tareas = faker.Generate(50)
                              .GroupBy(t => t.nombreTarea.Replace(" ", "").ToUpper())
                              .Select(g => g.First())
                              .ToList();

            context.Tareas.AddRange(tareas);
            context.SaveChanges();
        }
    }
}
