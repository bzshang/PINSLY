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

namespace DataOperations
{
    public class AccelerometerProvider
    {
        private Accelerometer _accelero;

        private IObservable<AccelerometerReadingChangedEventArgs> _observable;

        public AccelerometerProvider()
        {
            _accelero = Accelerometer.GetDefault();

            // https://msdn.microsoft.com/en-us/library/Hh229241(v=VS.103).aspx
            // Never finished understanding it, but just copy/paste it for now.

            _observable = Observable.FromEvent<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>, AccelerometerReadingChangedEventArgs>(
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

            return _observable.Subscribe(ReadingChangedCallBack);
        }


    }
}
