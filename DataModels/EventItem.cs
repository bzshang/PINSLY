using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class EventItem
    {
        public string StreamName { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double Value { get; set; }


    }
}
