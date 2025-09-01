namespace crudsweb3.Seeders
{
    public interface IDbInitializer
    {
        void Initialize(IServiceProvider serviceProvider, bool reset = false);
    }
}
//esta interfaz define el contrato para inicializar (sembrar) datos en la base de datos
//nos permite tener una clase Seeder que implemente este método y ser usada en Program.cs