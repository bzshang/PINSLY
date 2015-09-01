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
        private QueueConsumer _queueConsumer;

        public DataProcessor()
        {
            _queue = new EventQueue();
            _queueProducer = new QueueProducer(_queue);
            _queueConsumer = new QueueConsumer(_queue);
        }

        public void Start()
        {
            Task.Run(() => _queueProducer.SubscribeAndProduce());
            Task.Run(() => _queueConsumer.Consume());
        }
    }
}
