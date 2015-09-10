using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataOperations
{
    public class DataProcessor
    {
        private EventQueue _queue;
        private QueueProducer _queueProducer;
        private QueueConsumersManager _queueConsumers;

        private CancellationTokenSource _cts;
        private Task _producer;
        private Task _consumer;

        public IList<QueueConsumer> Consumers
        {
            get
            {
                return _queueConsumers.Consumers;
            }
        }

        public DataProcessor(DataContext dataContext)
        {
            _queue = new EventQueue();
            _queueProducer = new QueueProducer(_queue, dataContext);
            _queueConsumers = new QueueConsumersManager(_queue);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();

            _producer = Task.Run(() => _queueProducer.SubscribeAndProduce(), _cts.Token);
            _consumer = Task.Run(async () => await _queueConsumers.BeginConsume(), _cts.Token);
        }

        public async Task Stop()
        {
            //_cts.Cancel();

            CleanupProducer();
            await CleanupConsumer();

            //_cts.Dispose();
        }

        private void CleanupProducer()
        {
            _queueProducer.Cleanup();
        }

        private async Task CleanupConsumer()
        {
            await _consumer;
            _queueConsumers.Cleanup();
        }
    }
}
