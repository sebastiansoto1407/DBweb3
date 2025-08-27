using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;

namespace crudsweb3.data
{
    public class TareaDbContext :  DbContext
    {
        public TareaDbContext(DbContextOptions <TareaDbContext> options) : base(options) {
            
        }
        public DbSet<Tarea> Tareas {  get; set; }

        protected TareaDbContext()
        {

        }
    }
}
