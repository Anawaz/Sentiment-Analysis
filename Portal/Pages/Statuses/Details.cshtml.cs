using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Models;

namespace Portal.Pages.Statuses
{
    public class DetailsModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public DetailsModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Status Status { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Status = await _context.Status.FirstOrDefaultAsync(m => m.ID == id);

            if (Status == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
