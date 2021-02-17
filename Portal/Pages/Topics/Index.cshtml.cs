using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Models;

namespace Portal.Pages.Topics
{
    public class IndexModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public IndexModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Topic> Topic { get;set; }

        public async Task OnGetAsync()
        {
            //Topic = await _context.Topic
            //    .Include(t => t.Status).ToListAsync();

            Topic = await (from t in _context.Topic
                           select new Topic
                           {
                               ID = t.ID,
                               Name = t.Name,
                               Keywords = t.Keywords,
                               StatusID = t.StatusID,
                               DT = t.DT,
                               TweetsCount = (from recc in _context.TwitterRecord where recc.TopicID == t.ID select recc.Id).Count(),
                           }).ToListAsync();
        }
    }
}
