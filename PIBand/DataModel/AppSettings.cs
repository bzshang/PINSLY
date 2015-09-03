using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PIBand.Data
{
    public class AppSettings
    {
        private ApplicationDataContainer _container;

        public IReadOnlyDictionary<string,ApplicationDataContainer> Containers
        {
            get
            {
                return _container.Containers;
            }
        }

        public ApplicationDataLocality Locality
        {
            get
            {
                return _container.Locality;
            }
        }

        public string Name
        {
            get
            {
                return _container.Name;
            }
        }

        public AppSettings(ApplicationDataContainer container)
        {
            _container = container;

        }

        public AppSettings CreateContainer(string name, ApplicationDataCreateDisposition disp)
        {
            ApplicationDataContainer newContainer = _container.CreateContainer(name, disp);
            return new AppSettings(newContainer);
        }

        public void DeleteContainer(string name)
        {
            _container.DeleteContainer(name);
        }

        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            if (_container.Values.ContainsKey(key))
            {
                value = (T)_container.Values[key];
            }
            else
            {
                value = defaultValue;
            }

            return value;
        }

        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;
 
            var dict = _container.Values;
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

            return valueChanged;
        }

    }


}
