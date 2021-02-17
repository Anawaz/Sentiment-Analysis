using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class LineChart
    {
        public string Name { get; set; }
        public int PositiveRecords { get; set; }
        public int NegativeRecords { get; set; }
        public int NeutralRecords { get; set; }
    }
}
