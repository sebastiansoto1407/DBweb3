using Microsoft.Extensions.DependencyInjection;

namespace crudsweb3.Seeders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, TareaSeeder>();
            return services;
        }
    }
}
// Esta clase de extension permite registrar facilmente el seeder en Program.cs.
// Llama a services.AddSeeders() para habilitar TareaSeeder en la inyeccion de dependencias.