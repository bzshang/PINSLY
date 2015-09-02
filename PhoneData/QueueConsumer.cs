using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModels;

namespace PhoneData
{
    public class QueueConsumer
    {
        private EventQueue _queue;

        public event EventHandler<PhoneDataEventArgs> EventsReceived;

        private IList<EventItem> _listItems;

        public QueueConsumer(EventQueue queue)
        {
            _queue = queue;
        }

        public void ConsumeEvents()
        {
            _listItems = new List<EventItem>();
            foreach (EventItem item in _queue.GetConsumingEnumerable())
            {
                _listItems.Add(item);

                if (_listItems.Count == 100)
                {
                    RaiseEvent(_listItems);
                    _listItems.Clear();
                }
            }

            ConsumeRemainder();

        }

        private void RaiseEvent(IList<EventItem> items)
        {
            PhoneDataEventArgs eventArgs = new PhoneDataEventArgs();
            eventArgs.Items = items;
            EventsReceived(this, eventArgs);
        }

        public void ConsumeRemainder()
        {
            RaiseEvent(_listItems);
        }
    }
}
