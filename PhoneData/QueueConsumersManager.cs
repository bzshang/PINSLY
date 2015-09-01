using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PhoneData
{
    public class QueueConsumersManager
    {
        private EventQueue _queue;

        private int _numConsumerTasks = 10;

        private IList<QueueConsumer> _consumers;

        public QueueConsumersManager(EventQueue queue)
        {
            _queue = queue;

            _consumers = Enumerable.Range(0, _numConsumerTasks).Select(i => new QueueConsumer(_queue)).ToList();

        }

        public void BeginConsume()
        {
            Task[] tasks = _consumers.Select(i => Task.Run(() => i.ConsumeEvents())).ToArray();               
        }


    }
}
