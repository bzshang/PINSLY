using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace PhoneData
{
    public class AccelerometerProvider : IDataProvider
    {
        private Accelerometer _accelero;

        public AccelerometerProvider()
        {
            _accelero = Accelerometer.GetDefault();
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
