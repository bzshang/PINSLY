using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;

namespace PhoneData
{
    public class SessionContext
    {
        public UserContext UserContext { get; set; }

        public DataContext DataContext { get; set; }
    }

    public class DataContext
    {
        public bool PhoneAccelerometerEnabled { get; set; }
        public bool PhoneGeopositionEnabled { get; set; }
    }

    public class UserContext
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Dictionary<StreamsEnum, string> WebIDs { get; set; }
    }


}
