using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DataModels;
using DataSender;
using PhoneData;

namespace PIBand.Data
{
    public class PhoneDataController
    {
        private DataProcessor _dataProcessor;

        private IList<PIWebClient> _piWebClients;

        public PhoneDataController()
        {
            SessionContext sessionCtx = GetSessionContext();
            _dataProcessor = new DataProcessor(sessionCtx.DataContext);

            _piWebClients = _dataProcessor.Consumers.Select(consumer => new PIWebClient(sessionCtx.UserContext, consumer)).ToList();
        }

        public void Initialize()
        {
            foreach (PIWebClient client in _piWebClients)
            {
                client.AttachToConsumerEvent();
            }

            _dataProcessor.Start();
        }

        private SessionContext GetSessionContext()
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

        public async Task Close()
        {
            await _dataProcessor.Stop();

            foreach (PIWebClient client in _piWebClients)
            {
                client.Dispose();
            }
        }

    }
}
