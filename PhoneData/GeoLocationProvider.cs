using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

using System.Reactive.Linq;

namespace PhoneData
{
    public class GeoLocationProvider : IDataProvider
    {
        private Geolocator _geolocator;
        //private DispatcherTimer _timer;

        private IObservable<PositionChangedEventArgs> _observable;

        public GeoLocationProvider()
        {
            _geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };
            _geolocator.ReportInterval = 1000;

            // https://msdn.microsoft.com/en-us/library/Hh229241(v=VS.103).aspx
            // Never finished understanding it, but just copy/paste it for now.

            _observable = Observable.FromEvent<TypedEventHandler<Geolocator, PositionChangedEventArgs>, PositionChangedEventArgs>(
                handler =>
                {
                    TypedEventHandler<Geolocator, PositionChangedEventArgs> typedEventHandler = (sender, e) =>
                    {
                        handler(e);
                    };

                    return typedEventHandler;
                },
                typedEventHandler => AttachEvent(typedEventHandler),
                typedEventHandler => DetachEvent(typedEventHandler));

            //_observable = Observable.Interval(TimeSpan.FromSeconds(10)).Select(async i => await _geolocator.GetGeopositionAsync());

            //_timer = new DispatcherTimer();
            //_timer.Tick += dispatcherTimer_Tick;
            //_timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void AttachEvent(TypedEventHandler<Geolocator, PositionChangedEventArgs> ev)
        {
            _geolocator.PositionChanged += ev;
        }

        private void DetachEvent(TypedEventHandler<Geolocator, PositionChangedEventArgs> ev)
        {
            _geolocator.ReportInterval = 0;
            _geolocator.PositionChanged -= ev;
        }

        public IDisposable SubscribeToObservable(Action<PositionChangedEventArgs> GeopositionReaction)
        {
            return _observable.Subscribe(GeopositionReaction);
        }

        public void Start()
        {
            //_timer.Start();
        }

        public void Stop()
        {
            //_timer.Stop();
        }

        //private void SubscribeToStatusChanged(TypedEventHandler<Geolocator, StatusChangedEventArgs> OnStatusChanged)
        //{
        //    _geolocator.StatusChanged += OnStatusChanged;
        //}

        //private void UnsubscribeToStatusChanged(TypedEventHandler<Geolocator, StatusChangedEventArgs> OnStatusChanged)
        //{
        //    _geolocator.StatusChanged -= OnStatusChanged;
        //}

        //public async Task<Geoposition> GetGeopositionAsync()
        //{
        //    return await _geolocator.GetGeopositionAsync();
        //}

        //public async void dispatcherTimer_Tick(object sender, object e)
        //{
        //    Geoposition geo = await GetGeopositionAsync();

        //    //double lat = geo.Coordinate.Point.Position.Latitude;
        //    double lat = geo.Coordinate.Timestamp.Second;
        //    double lng = geo.Coordinate.Point.Position.Longitude;

        //    DateTimeOffset ts = geo.Coordinate.Timestamp;

        //    EventItem latEvent = new EventItem { StreamName = "accX", Timestamp = ts, Value = lat };
        //    EventItem lngEvent = new EventItem { StreamName = "accY", Timestamp = ts, Value = lng };

        //    IList<EventItem> events = new[] { latEvent, lngEvent};

        //    SendToQueue(events);
        //}

        private void SendToQueue(IList<EventItem> events)
        {



        }

    }
}
