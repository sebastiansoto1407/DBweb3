using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;
using crudsweb3.data;
using System;
using System.Text.RegularExpressions;

namespace crudsweb3.Pages
{
    public class EditModel : PageModel
    {
        private readonly TareaDbContext _context;

        public EditModel(TareaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var tarea = await _context.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tarea == null)
                return NotFound();

            Tarea = tarea;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Tarea.nombreTarea = (Tarea?.nombreTarea ?? string.Empty);
            Tarea.estado = (Tarea?.estado ?? string.Empty).Trim();

            Tarea.nombreTarea = Regex.Replace(Tarea.nombreTarea, @"\s+", " ").Trim();

            if (string.IsNullOrWhiteSpace(Tarea.nombreTarea))
                ModelState.AddModelError("Tarea.nombreTarea", "El nombre de la tarea es obligatorio.");

            if (!Regex.IsMatch(Tarea.nombreTarea, @"^[\p{L}\s]+$"))
                ModelState.AddModelError("Tarea.nombreTarea", "El nombre solo puede contener letras y espacios (sin puntos ni símbolos).");

            var letras = Regex.Matches(Tarea.nombreTarea, @"\p{L}").Count;
            if (letras < 3)
                ModelState.AddModelError("Tarea.nombreTarea", "El nombre debe tener al menos 3 letras.");

            if (string.IsNullOrWhiteSpace(Tarea.estado))
                ModelState.AddModelError("Tarea.estado", "El estado es obligatorio.");

            if (Tarea.fechaVencimiento == default)
                ModelState.AddModelError("Tarea.fechaVencimiento", "La fecha y hora de vencimiento son obligatorias.");
            else if (Tarea.fechaVencimiento < DateTime.Now)
                ModelState.AddModelError("Tarea.fechaVencimiento", "La fecha y hora de vencimiento no pueden ser anteriores a ahora.");

            if (Tarea.IdUsuario <= 0)
                ModelState.AddModelError("Tarea.IdUsuario", "El usuario debe ser un número mayor que cero.");

            if (!ModelState.IsValid)
                return Page();

            string nombreSinEspaciosUpper = Tarea.nombreTarea.Replace(" ", string.Empty).ToUpper();

            bool yaExiste = await _context.Tareas
                .AsNoTracking()
                .AnyAsync(t =>
                    t.Id != Tarea.Id &&
                    ((t.nombreTarea ?? "")
                        .Replace(" ", "")
                        .ToUpper()) == nombreSinEspaciosUpper
                );

            if (yaExiste)
            {
                ModelState.AddModelError("Tarea.nombreTarea", "Ya existe una tarea con el mismo nombre.");
                return Page();
            }

            _context.Attach(Tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(Tarea.Id))
                    return NotFound();

                ModelState.AddModelError(string.Empty, "Otro usuario modificó o eliminó este registro. Vuelve a intentar.");
                var fresh = await _context.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == Tarea.Id);
                if (fresh != null) Tarea = fresh;
                return Page();
            }
        }

        private bool TareaExists(int id) =>
            _context.Tareas.Any(e => e.Id == id);
    }
}
