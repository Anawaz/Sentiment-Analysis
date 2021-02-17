using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.Models;

namespace Portal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Portal.Models.Topic> Topic { get; set; }
        public DbSet<Portal.Models.Status> Status { get; set; }
        public DbSet<Portal.Models.TwitterRecord> TwitterRecord { get; set; }


    }
}
