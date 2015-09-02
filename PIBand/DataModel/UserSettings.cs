using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIBand.Data
{
    public class UserSettings
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string PIWebAPIServer { get; private set; }
        public string AFServer { get; private set; }
        public string PIServer { get; private set; }

        public UserSettings(string userName, string password, string piwebapiServer, string afServer, string piServer)
        {
            this.Username = userName;
            this.Password = password;
            this.PIWebAPIServer = piwebapiServer;
            this.AFServer = afServer;
            this.PIServer = piServer;
        }

    }
}
