using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class EventItem
    {
        public StreamsEnum Stream { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double Value { get; set; }


    }
}
