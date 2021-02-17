using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Models;

namespace Portal.Pages.Topics
{
    [EnableCors("MyPolicy")]
    public class GrabTweetModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public GrabTweetModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public Topic Topic { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Topic = await _context.Topic
                .Include(t => t.Status).FirstOrDefaultAsync(m => m.ID == id);
            

            if (Topic == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
