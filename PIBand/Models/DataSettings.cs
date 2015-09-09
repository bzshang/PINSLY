using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIBand.Models
{
    public class DataSettings
    {
        public BandSettings BandSettings { get; private set; }
        public PhoneSettings PhoneSettings { get; private set; }

        public static DataSettings GetStoredDataSettings()
        {
            AppSettings bandSettingsContainer = AppSettingsManager.Instance.LocalSettings.Containers["BandSettings"];
            AppSettings phoneSettingsContainer = AppSettingsManager.Instance.LocalSettings.Containers["PhoneSettings"];

            BandSettings bandSettings = new BandSettings
            {
                IsLinked = bandSettingsContainer.GetValueOrDefault("IsLinked", false)
            };

            PhoneSettings phoneSettings = new PhoneSettings
            {
                AccelerometerEnabled = phoneSettingsContainer.GetValueOrDefault("Accelerometer", true),
                GeopositionEnabled = phoneSettingsContainer.GetValueOrDefault("Geoposition", true)
            };

            return new DataSettings
            {
                BandSettings = bandSettings,
                PhoneSettings = phoneSettings
            };
        }

    }

    public class PhoneSettings
    {
        public bool AccelerometerEnabled { get; set; }
        public bool GeopositionEnabled { get; set; }
    }

    public class BandSettings
    {
        public bool IsLinked { get; set; }
    }
}
