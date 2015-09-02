using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneData
{
    public class SessionContext
    {
        public UserContext UserContext { get; set; }

        public DataContext DataContext { get; set; }
    }
}
