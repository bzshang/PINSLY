using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public static class StreamsMapper
    {

        public static StreamsEnum MapToEnum(string name)
        {
            if (name.Contains("PhoneAccelerometerX"))
            {
                return StreamsEnum.PhoneAccelerometerX;
            }
            if (name.Contains("PhoneAccelerometerY"))
            {
                return StreamsEnum.PhoneAccelerometerY;
            }
            if (name.Contains("PhoneAccelerometerZ"))
            {
                return StreamsEnum.PhoneAccelerometerZ;
            }
            if (name.Contains("Geoposition_Latitude"))
            {
                return StreamsEnum.GeopositionLatitude;
            }
            if (name.Contains("Geoposition_Longitude"))
            {
                return StreamsEnum.GeopositionLongitude;
            }
            return StreamsEnum.None;
        }

    }
}
