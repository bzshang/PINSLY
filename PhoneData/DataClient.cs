using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataOperations;

using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

namespace DataOperations
{
    public class DataClient
    {
        private DataManager _dataManager;

        private IList<IDisposable> _tokens;

        public DataClient()
        {
            _dataManager = DataManager.Instance;
            _tokens = new List<IDisposable>();
        }

        public void SubscribeToAccelerometer(Action<AccelerometerReadingChangedEventArgs> CallBack)
        {
            IDisposable token = _dataManager.SubscribeToAccelerometer(CallBack);
            _tokens.Add(token); 
        }

        public void SubscribeToGeolocation(Action<PositionChangedEventArgs> CallBack)
        {
            IDisposable token = _dataManager.SubscribeToGeolocation(CallBack);
            _tokens.Add(token);
        }

        public void RemoveSubscription()
        {
            foreach (IDisposable token in _tokens)
            {
                token.Dispose();
            }
        }
    }
}
