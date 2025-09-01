using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;
using crudsweb3.data;

namespace crudsweb3.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly TareaDbContext _context;

        public DeleteModel(TareaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return RedirectToPage("./Index");
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
