using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Data;
using Portal.Models;

namespace Portal.Pages.Topics
{
    public class CreateModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public CreateModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["StatusID"] = new SelectList(_context.Status, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public Topic Topic { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Topic.StatusID = 1;
            Topic.DT = DateTime.Now;
            _context.Topic.Add(Topic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
