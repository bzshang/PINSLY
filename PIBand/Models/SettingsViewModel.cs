using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIBand.Models
{
    public class SettingsViewModel
    {
        public string Name { get; private set; }

        public UserSettings UserSettings { get; private set; }

        public BandSettings BandSettings { get; private set; }

        public PhoneSettings PhoneSettings { get; private set; }
    }
}
