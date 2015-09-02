using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIBand.Data
{
    public class DataSettings
    {
        public bool AccelerometerEnabled { get; set; }
        public bool GeopositionEnabled { get; set; }

        public DataSettings(bool accelerometer, bool geoposition)
        {
            AccelerometerEnabled = accelerometer;
            GeopositionEnabled = geoposition;
        }

    }
}
