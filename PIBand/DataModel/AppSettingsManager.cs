using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PIBand.Data
{
    public static class AppSettingsManager
    {
        private static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private static string _containerName = "appContainer";

        private static ApplicationDataContainer container = _localSettings.CreateContainer(_containerName, ApplicationDataCreateDisposition.Always);

        public static T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            bool hasContainer = _localSettings.Containers.ContainsKey(_containerName);

            if (hasContainer)
            {
                if (_localSettings.Containers[_containerName].Values.ContainsKey(key))
                {
                    value = (T)_localSettings.Containers[_containerName].Values[key];
                }
                else
                {
                    value = defaultValue;
                }
                return value;
            }

            return defaultValue;
        }

        public static bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            bool hasContainer = _localSettings.Containers.ContainsKey(_containerName);

            if (hasContainer)
            {
                var dict = _localSettings.Containers[_containerName].Values;
                // If the key exists
                if (dict.ContainsKey(key))
                {
                    // If the value has changed
                    if (dict[key] != value)
                    {
                        // Store the new value
                        dict[key] = value;
                        valueChanged = true;
                    }
                }
                // Otherwise create the key.
                else
                {
                    dict.Add(key, value);
                    valueChanged = true;
                }
            }

            return valueChanged;
        }

        public static UserSettings GetUserSettings()
        {
            return new UserSettings(
                GetValueOrDefault("Username", "PINSly_user"),
                GetValueOrDefault("Password", "secret"),
                GetValueOrDefault("PI Web API Server", "osiproghack01.cloudapp.net"),
                GetValueOrDefault("AF Server", "testAFServer"),
                GetValueOrDefault("PI Server", "testPIServer"));
        }

        public static DataSettings GetDataSettings()
        {
            return new DataSettings(
                GetValueOrDefault("Accelerometer", true),
                GetValueOrDefault("Accelerometer", true));
        }
    }


}
