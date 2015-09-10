using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Security.Credentials;

namespace PIBand.Models
{
    public class AppSettings
    {
        private ApplicationDataContainer _container;

        public IReadOnlyDictionary<string, AppSettings> Containers
        {
            get
            {
                return _container.Containers.ToDictionary(i => i.Key, i => new AppSettings(i.Value));
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

        public string GetPassword(string currentUser)
        {
            IReadOnlyList<PasswordCredential> credentialList = null;

            var vault = new PasswordVault();
            try
            {
                credentialList = vault.FindAllByUserName(currentUser);
            }
            catch
            {
                return "";
            }

            PasswordCredential credential = null;
            if (credentialList.Count > 0)
            {
                credential = credentialList[0];
            }
            credential.RetrievePassword();

            return credential.Password;
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

        public bool AddOrUpdatePassword(string currentUser, string password)
        {
            PasswordCredential credential = null;
            var vault = new PasswordVault();

            IReadOnlyList<PasswordCredential> credentialList = null;
            try
            {
                credentialList = vault.FindAllByUserName(currentUser);
            }
            catch
            {       
                vault.Add(new PasswordCredential("PINSly", currentUser, password));
                return true;
            }

            if (credentialList.Count > 0)
            {
                credential = credentialList[0];
            }
            credential.RetrievePassword();

            if (credential.Password != password)
            {
                vault.Remove(new PasswordCredential("PINSly", currentUser, credential.Password));
                vault.Add(new PasswordCredential("PINSly", currentUser, password));
                return true;
            }

            return false;

        }

    }


}
