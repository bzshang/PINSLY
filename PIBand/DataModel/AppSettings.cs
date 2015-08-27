using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PIBand.Data
{
    public static class AppSettings
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

        public static ConfigSettings GetSettings()
        {

            return new ConfigSettings(
                GetValueOrDefault("Username", "PINSly_user"),
                GetValueOrDefault("Password", "secret"),
                GetValueOrDefault("PI WebAPI Server", "osiproghackuc2015.cloudapp.net"),
                GetValueOrDefault("AF Server", "testAFServer"),
                GetValueOrDefault("PI Server", "testPIServer"));

            //var settingsGroup = new SettingsDataGroup("Settings");
       
            //settingsGroup.Items.Add(new SettingsDataItem("Username", GetValueOrDefault("Username", "PINSly_user")));
            //settingsGroup.Items.Add(new SettingsDataItem("Password", GetValueOrDefault("Password", "secret")));
            //settingsGroup.Items.Add(new SettingsDataItem("PI Web API Server", GetValueOrDefault("PI WebAPI Server", "osiproghackuc2015.cloudapp.net")));
            //settingsGroup.Items.Add(new SettingsDataItem("AF Server", GetValueOrDefault("AF Server", "testAFServer")));
            //settingsGroup.Items.Add(new SettingsDataItem("PI Server", GetValueOrDefault("PI Server", "testPIServer")));

            //return settingsGroup;
        }
    }

    public class SettingsDataItem
    {
        public SettingsDataItem(String key, String itemValue)
        {
            this.Key = key;
            this.ItemValue = itemValue;
        }

        public string Key { get; private set; }
        public string ItemValue { get; private set; }

        public override string ToString()
        {
            return this.Key + ":" + this.ItemValue;
        }

    }

    public class SettingsDataGroup
    {
        public SettingsDataGroup(String name)
        {
            this.Name = name;
            this.Items = new ObservableCollection<SettingsDataItem>();
        }

        public string Name { get; private set; }
        public ObservableCollection<SettingsDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class ConfigSettings
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string PIWebAPIServer { get; private set; }
        public string AFServer { get; private set; }
        public string PIServer { get; private set; }

        public ConfigSettings(string userName, string password, string piwebapiServer, string afServer, string piServer)
        {
            this.Username = userName;
            this.Password = password;
            this.PIWebAPIServer = piwebapiServer;
            this.AFServer = afServer;
            this.PIServer = piServer;
        }

    }
}
