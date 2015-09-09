using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;

namespace PhoneData
{
    public class DataEventArgs : EventArgs
    {
        public IList<EventItem> Items { get; set; }

    }
}
