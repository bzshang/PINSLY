using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

namespace DataOperations
{
    public class DataManager
    {
        private static readonly Lazy<DataManager> lazyDataManager = new Lazy<DataManager>(() => new DataManager());

        public static DataManager Instance { get { return lazyDataManager.Value; } }

        private AccelerometerProvider _acceleroProvider;
        private GeoLocationProvider _geoProvider;

        private DataManager()
        {
            _acceleroProvider = new AccelerometerProvider();
            _geoProvider = new GeoLocationProvider();
        }

        public IDisposable SubscribeToAccelerometer(Action<AccelerometerReadingChangedEventArgs> CallBack)
        {
            return _acceleroProvider.SubscribeToObservable(CallBack);
        }

        public IDisposable SubscribeToGeolocation(Action<PositionChangedEventArgs> CallBack)
        {
            return _geoProvider.SubscribeToObservable(CallBack);
        }



    }
}
