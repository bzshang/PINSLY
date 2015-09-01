using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneData
{
    public class DataProcessor
    {
        private EventQueue _queue;
        private QueueProducer _queueProducer;
        private QueueConsumersManager _queueConsumers;

        public DataProcessor()
        {
            _queue = new EventQueue();
            _queueProducer = new QueueProducer(_queue);
            _queueConsumers = new QueueConsumersManager(_queue);
        }

        public void Start()
        {
            Task.Run(() => _queueProducer.SubscribeAndProduce());
            Task.Run(() => _queueConsumers.BeginConsume());
        }
    }
}
