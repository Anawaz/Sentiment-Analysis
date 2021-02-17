using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portal.Pages {
    public class IndexModel : PageModel {
        private readonly Portal.Data.ApplicationDbContext _context;

        public IndexModel(Portal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["TotalTopics"] = _context.Topic.Count().ToString();
            ViewData["TotalRecords"] = _context.TwitterRecord.Count().ToString();
            ViewData["PositiveTweets"] = _context.TwitterRecord.Where(x => x.Sentiment == "positive").Count().ToString();
            ViewData["NegativeTweets"] = _context.TwitterRecord.Where(x => x.Sentiment == "negative").Count().ToString();
            ViewData["NeutralTweets"] = _context.TwitterRecord.Where(x => x.Sentiment == "neutral").Count().ToString();
            ViewData["PendingTweets"] = _context.TwitterRecord.Where(x => x.Sentiment == "pending").Count().ToString();
            return Page();
        }

    }
}
