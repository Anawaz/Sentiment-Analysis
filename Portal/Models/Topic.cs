using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Topic
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public int StatusID { get; set; }
        public Status Status { get; set; }

        public DateTime DT { get; set; }

        [NotMapped]
        public Int64 TweetsCount { get; set; }
    }
}
