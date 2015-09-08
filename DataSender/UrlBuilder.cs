using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSender
{
    public static class UrlBuilder
    {
        private static readonly string _server = "osiproghack01.cloudapp.net";

        public static string Recorded(string webID)
        {
            return string.Format("https://osiproghack01.cloudapp.net/piwebapi/streams/{webID}/recorded", webID);
            //return @"https://osiproghack01.cloudapp.net/piwebapi/streams/P0LxDSL0sCwEC3yUrXhcG-8A5wMAAASlVQSVRFUjAwMVxBQ0NFTFg/recorded:;
        }

        public static string UpdateValuesAdHoc()
        {
            return @"https://osiproghack01.cloudapp.net/piwebapi/streamsets/recorded";

        }

    }
}
