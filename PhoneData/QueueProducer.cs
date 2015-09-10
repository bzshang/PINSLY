using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Sensors;
using Windows.Devices.Geolocation;

using System.Diagnostics;

using DataModels;

namespace DataOperations
{
    public class QueueProducer
    {
        private EventQueue _queue;

        private DataClient _dataClient;

        private DataContext _dataContext;

        public QueueProducer(EventQueue queue, DataContext dataContext)
        {
            _dataClient = new DataClient();
            _queue = queue;
            _dataContext = dataContext;
        }

        public void SubscribeAndProduce()
        {
            if (_dataContext.PhoneAccelerometerEnabled)
                _dataClient.SubscribeToAccelerometer(OnPhoneAccelerometerReading);
            if (_dataContext.PhoneGeopositionEnabled)
                _dataClient.SubscribeToGeolocation(OnPhoneGeopositionReading);
        }

        private void OnPhoneAccelerometerReading(AccelerometerReadingChangedEventArgs args)
        {
            AccelerometerReading reading = args.Reading;
            DateTimeOffset ts = reading.Timestamp;
            EventItem accX = new EventItem { Stream = StreamsEnum.PhoneAccelerometerX, Timestamp = ts, Value = reading.AccelerationX };
            EventItem accY = new EventItem { Stream = StreamsEnum.PhoneAccelerometerY, Timestamp = ts, Value = reading.AccelerationY };
            EventItem accZ = new EventItem { Stream = StreamsEnum.PhoneAccelerometerZ, Timestamp = ts, Value = reading.AccelerationZ };

            //IList<EventItem> events = new[] { accX };
            IList<EventItem> events = new[] { accX, accY, accZ };
            //Debug.WriteLine(DateTime.Now);
            SendToQueue(events);
        }

        public void OnPhoneGeopositionReading(PositionChangedEventArgs args)
        {
            Geoposition geo = args.Position;

            double lat = geo.Coordinate.Point.Position.Latitude;
            double lng = geo.Coordinate.Point.Position.Longitude;

            DateTimeOffset ts = geo.Coordinate.Timestamp;

            EventItem latEvent = new EventItem { Stream = StreamsEnum.GeopositionLatitude, Timestamp = ts, Value = lat };
            EventItem lngEvent = new EventItem { Stream = StreamsEnum.GeopositionLongitude, Timestamp = ts, Value = lng };

            IList<EventItem> events = new[] { latEvent, lngEvent };

            //SendToQueue(events);
        }

        private void SendToQueue(IList<EventItem> events)
        {
            foreach (EventItem eventItem in events)
            {
                if (!_queue.IsAddingCompleted)
                    _queue.Add(eventItem);
            }
        }

        public void Cleanup()
        {       
            _dataClient.RemoveSubscription();
            _queue.CompleteAdding();
        }

    }
}
