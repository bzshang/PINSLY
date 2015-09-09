using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DataSender;
using PhoneData;

using PIBand.Models;

namespace PIBand.Controllers
{
    public class PhoneDataController
    {
        private DataProcessor _dataProcessor;

        private IList<PIWebClient> _piWebClients;

        public PhoneDataController()
        {
            SessionContext sessionCtx = SessionContextProvider.GetSessionContext();
            _dataProcessor = new DataProcessor(sessionCtx.DataContext);

            _piWebClients = _dataProcessor.Consumers.Select(consumer => new PIWebClient(sessionCtx.UserContext, consumer)).ToList();
        }

        public void Initialize()
        {
            foreach (PIWebClient client in _piWebClients)
            {
                client.AttachToConsumerEvent();
            }

            _dataProcessor.Start();
        }

        public async Task Close()
        {
            await _dataProcessor.Stop();

            foreach (PIWebClient client in _piWebClients)
            {
                client.Dispose();
            }
        }

    }
}
