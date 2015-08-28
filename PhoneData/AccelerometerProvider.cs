using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

using System.Reactive;
using System.Reactive.Linq;

namespace PhoneData
{
    public class AccelerometerProvider : IDataProvider
    {
        private Accelerometer _accelero;

        private IObservable<AccelerometerReadingChangedEventArgs> _observableAccelero;

        public AccelerometerProvider()
        {
            _accelero = Accelerometer.GetDefault();

            // https://msdn.microsoft.com/en-us/library/Hh229241(v=VS.103).aspx
            // Never finished understanding it, but just copy/paste it for now.

            _observableAccelero = Observable.FromEvent<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>, AccelerometerReadingChangedEventArgs>(
                handler =>
                {
                    TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> typedEventHandler = (sender, e) =>
                    {
                        handler(e);
                    };

                    return typedEventHandler;
                },
                typedEventHandler => AttachEvent(typedEventHandler),
                typedEventHandler => DetachEvent(typedEventHandler));

            // Doesn't work
            //_observableAccelero = Observable.FromEvent<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>, AccelerometerReadingChangedEventArgs>(
            //ev => _accelero.ReadingChanged += ev,
            //ev => _accelero.ReadingChanged -= ev);
        }

        private void AttachEvent(TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ev)
        {
            _accelero.ReadingChanged += ev;
        }

        private void DetachEvent(TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ev)
        {
            _accelero.ReportInterval = 0;
            _accelero.ReadingChanged -= ev;
        }

        public IDisposable SubscribeToObservable(Action<AccelerometerReadingChangedEventArgs> ReadingChangedCallBack)
        {
            uint minReportInterval = _accelero.MinimumReportInterval;
            uint desiredReportInterval = minReportInterval > 16 ? minReportInterval : 16;
            _accelero.ReportInterval = desiredReportInterval;

            return _observableAccelero.Subscribe(ReadingChangedCallBack);
        }

        public void Subscribe(TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ReadingChangedCallBack)
        {
            _accelero.ReadingChanged += ReadingChangedCallBack;
        }

        public void Unsubscribe(TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ReadingChangedCallBack)
        {
            _accelero.ReadingChanged -= ReadingChangedCallBack;
        }

        public void Start()
        {
            uint minReportInterval = _accelero.MinimumReportInterval;
            uint desiredReportInterval = minReportInterval > 16 ? minReportInterval : 16;
            _accelero.ReportInterval = desiredReportInterval;
        }

        public void Stop()
        {
            _accelero.ReportInterval = 0;
        }

        private void OnReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            AccelerometerReading reading = args.Reading;
            DateTimeOffset ts = reading.Timestamp;
            EventItem accX = new EventItem { StreamName = "accX", Timestamp = ts, Value = reading.AccelerationX };
            EventItem accY = new EventItem { StreamName = "accY", Timestamp = ts, Value = reading.AccelerationY };
            EventItem accZ = new EventItem { StreamName = "accZ", Timestamp = ts, Value = reading.AccelerationZ };

            IList<EventItem> events = new[] { accX, accY, accZ };

            SendToQueue(events);
        }

        private void SendToQueue(IList<EventItem> events)
        {



        }
    }
}
