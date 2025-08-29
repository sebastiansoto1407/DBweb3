using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using crudsweb3.Models;
using crudsweb3.data;

namespace crudsweb3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly crudsweb3.data.TareaDbContext _context;

        public IndexModel(crudsweb3.data.TareaDbContext context)
        {
            _context = context;
        }

        public IList<Tarea> Tarea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas.ToListAsync();
        }
    }
}
