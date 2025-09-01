using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;
using crudsweb3.data;

namespace crudsweb3.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly TareaDbContext _context;

        public DetailsModel(TareaDbContext context)
        {
            _context = context;
        }

        public Tarea Tarea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await _context.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tarea == null) return NotFound();

            Tarea = tarea;
            return Page();
        }
    }
}
