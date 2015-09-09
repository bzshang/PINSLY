using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PhoneData;
using PIBand.Models;

namespace PIBand
{
    public static class SessionContextProvider
    {
        public static SessionContext GetSessionContext()
        {
            UserSettings userSettings = UserSettings.GetStoredUserSettings();
            DataSettings dataSettings = DataSettings.GetStoredDataSettings();

            UserContext userContext = new UserContext
            {
                Username = userSettings.Username,
                Password = userSettings.Password,
                WebIDs = userSettings.WebIDs
            };

            DataContext dataContext = new DataContext
            {
                PhoneAccelerometerEnabled = dataSettings.PhoneSettings.AccelerometerEnabled,
                PhoneGeopositionEnabled = dataSettings.PhoneSettings.GeopositionEnabled
            };

            SessionContext sessionContext = new SessionContext { UserContext = userContext, DataContext = dataContext };
            return sessionContext;
        }

    }
}
