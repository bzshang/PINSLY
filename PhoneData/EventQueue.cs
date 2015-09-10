using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

using DataModels;

namespace DataOperations
{
    public class EventQueue
    {
        private BlockingCollection<EventItem> _blockingQueue;

        public bool IsAddingCompleted
        {
            get
            {
                return _blockingQueue.IsAddingCompleted;
            }
        }

        public EventQueue()
        {
            _blockingQueue = new BlockingCollection<EventItem>(1000);
        }

        public void Add(EventItem eventItem)
        {
            _blockingQueue.Add(eventItem);
            Debug.WriteLine(_blockingQueue.Count);
        }

        public IEnumerable<EventItem> GetConsumingEnumerable()
        {
            //Debug.WriteLine(_blockingQueue.Count);
            //IEnumerable<EventItem> evts = _blockingQueue.Take(10);
            return _blockingQueue.GetConsumingEnumerable();
            //Debug.WriteLine(_blockingQueue.Count);
            //return evts;
        }


        public void CompleteAdding()
        {
            _blockingQueue.CompleteAdding();
        }

       

    }
}
