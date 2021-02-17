using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Models;

namespace Portal.Pages.Tweets
{
    public class IndexModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public IndexModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TwitterRecord> Tweets { get; set; }

        public async Task OnGetAsync()
        {
            Tweets = await _context.TwitterRecord.ToListAsync();
        }
    }
}
