using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;
using Windows.Storage;

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
            string displayName = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"]
                .GetValueOrDefault("DisplayName", "defaultUser");
            string userName = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"]
                .GetValueOrDefault("Username", "defaultUser");
            string password = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"]
                .GetPassword(userName);

            ApplicationDataCompositeValue webIds = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"]
                .GetValueOrDefault<ApplicationDataCompositeValue>("WebIds", null);

            Dictionary<StreamsEnum, string> webIdDict = ConvertToDict(webIds);

            return new UserSettings { DisplayName = displayName, Username = userName, Password = password, WebIDs = webIdDict };

        }

        private static Dictionary<StreamsEnum, string> ConvertToDict(ApplicationDataCompositeValue webIds)
        {
            if (webIds == null) return null;

            Dictionary<StreamsEnum, string> webIdDict =  new Dictionary<StreamsEnum, string>();
            foreach (var setting in webIds)
            {
                webIdDict.Add(StreamsMapper.MapToEnum(setting.Key), setting.Value as string);
            }
            return webIdDict;
        }

    }
}
