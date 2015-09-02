using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;

namespace PhoneData
{
    public class UserContext
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Dictionary<StreamsEnum,string> WebIDs { get; set; }
        
    }
}
