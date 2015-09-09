using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;

namespace PIBand.Models
{
    public class UserSettings
    {
        public string DisplayName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Dictionary<StreamsEnum, string> WebIDs { get; set; }

        public static UserSettings GetStoredUserSettings()
        {
            string displayName = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetValueOrDefault("DisplayName", "defaultUser");
            string userName = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetValueOrDefault("Username", "defaultUser");
            string password = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetPassword(userName);

            Dictionary<StreamsEnum, string> webIds = AppSettingsManager.Instance.LocalSettings
                .Containers["WebIDs"].GetValueOrDefault<Dictionary<StreamsEnum, string>>("Username", null);

            return new UserSettings { DisplayName = displayName, Username = userName, Password = password, WebIDs = webIds };

        }

    }
}
