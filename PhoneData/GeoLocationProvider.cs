using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace PhoneData
{
    public class GeoLocationProvider : IDataProvider
    {
        private Geolocator _geolocator;
        private DispatcherTimer _timer;

        public GeoLocationProvider()
        {
            _geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };
            _timer = new DispatcherTimer();
            _timer.Tick += dispatcherTimer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);

        }
        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void SubscribeToStatusChanged(TypedEventHandler<Geolocator, StatusChangedEventArgs> OnStatusChanged)
        {
            _geolocator.StatusChanged += OnStatusChanged;
        }

        private void UnsubscribeToStatusChanged(TypedEventHandler<Geolocator, StatusChangedEventArgs> OnStatusChanged)
        {
            _geolocator.StatusChanged -= OnStatusChanged;
        }

        public async Task<Geoposition> GetGeopositionAsync()
        {
            return await _geolocator.GetGeopositionAsync();
        }

        public async void dispatcherTimer_Tick(object sender, object e)
        {
            Geoposition geo = await GetGeopositionAsync();

            double lat = geo.Coordinate.Point.Position.Latitude;
            double lng = geo.Coordinate.Point.Position.Longitude;

            DateTimeOffset ts = geo.Coordinate.Timestamp;

            EventItem latEvent = new EventItem { StreamName = "accX", Timestamp = ts, Value = lat };
            EventItem lngEvent = new EventItem { StreamName = "accY", Timestamp = ts, Value = lng };

            IList<EventItem> events = new[] { latEvent, lngEvent};

            SendToQueue(events);
        }

        private void SendToQueue(IList<EventItem> events)
        {



        }

    }
}
