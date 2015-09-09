using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

using PIBand;

namespace PIBand.Models
{
    public sealed class AppSettingsManager
    {
        // http://csharpindepth.com/articles/general/singleton.aspx
        public AppSettings LocalSettings { get; }

        private static readonly AppSettingsManager instance = new AppSettingsManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppSettingsManager()
        {
        }

        private AppSettingsManager()
        {
            LocalSettings = new AppSettings(ApplicationData.Current.LocalSettings);
        }

        public static AppSettingsManager Instance
        {
            get
            {
                return instance;
            }
        }

        //public void Add(string name)
        //{
        //    _settings.Add(name, new AppSettings(name));
        //}

        //public void Remove(string name)
        //{
        //    _settings.Remove(name);
        //}

        //public AppSettings this[string name]
        //{
        //    get
        //    {
        //        return _settings[name];
        //    }         
        //}


    }
}
