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
    public class AnalysisModel : PageModel
    {
        private readonly Portal.Data.ApplicationDbContext _context;

        public AnalysisModel(Portal.Data.ApplicationDbContext context)
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

            ViewData["id"] = id;

            if (Topic == null)
            {
                return NotFound();
            }

            //var analyzer = new TagCloudAnalyzer();
            //var tweetsText = _context.TwitterRecord.Where(m => m.TopicID == id).Select(m => m.TweetText);
            //var tags = analyzer.ComputeTagCloud(tweetsText);
            //tags = tags.Shuffle();
           // ViewData["Tags"] = Newtonsoft.Json.JsonConvert.SerializeObject(tags);

            return Page();
        }
    }
}
