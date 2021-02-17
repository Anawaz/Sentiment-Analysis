using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Status.Any())
            {
                return;   // DB has been seeded
            }

            var statuses = new Status[]
            {
                new Status{Name ="Created",isActive=true},
                new Status{Name ="Tweets_Getting",isActive=true },
                new Status{Name ="Pending_Analysis",isActive=true},
                new Status{Name ="Analysis_Done",isActive=true}
            };

            context.Status.AddRange(statuses);
            context.SaveChanges();
        }
    }
}
