using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using crudsweb3.Models;
using crudsweb3.data;

namespace crudsweb3.Pages
{
    public class CreateModel : PageModel
    {
        private readonly crudsweb3.data.TareaDbContext _context;

        public CreateModel(crudsweb3.data.TareaDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Tareas.Add(Tarea);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
