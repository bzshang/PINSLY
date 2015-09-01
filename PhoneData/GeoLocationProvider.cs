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
    public class GeoLocationProvider
    {
        private Geolocator _geolocator;

        private IObservable<PositionChangedEventArgs> _observable;

        public GeoLocationProvider()
        {
            _geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };         

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
        }

        private void AttachEvent(TypedEventHandler<Geolocator, PositionChangedEventArgs> ev)
        {
            _geolocator.ReportInterval = 1000;
            _geolocator.PositionChanged += ev;
        }

        private void DetachEvent(TypedEventHandler<Geolocator, PositionChangedEventArgs> ev)
        {   
            _geolocator.PositionChanged -= ev;
            _geolocator.ReportInterval = 0;
        }

        public IDisposable SubscribeToObservable(Action<PositionChangedEventArgs> GeopositionReaction)
        {
            return _observable.Subscribe(args => GeopositionReaction(args));
        }



    }
}
