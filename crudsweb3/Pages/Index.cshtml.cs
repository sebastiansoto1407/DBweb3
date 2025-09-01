using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;
using crudsweb3.data;

namespace crudsweb3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TareaDbContext _context;

        public IndexModel(TareaDbContext context)
        {
            _context = context;
        }

        public IList<Tarea> Tarea { get; private set; } = new List<Tarea>();

        public Tarea Header { get; } = new Tarea();
        //refleja
        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas
                .AsNoTracking()
                .OrderBy(t => t.fechaVencimiento)
                .ThenBy(t => t.nombreTarea)
                .ToListAsync();
        }
    }
}
