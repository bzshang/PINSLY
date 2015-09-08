using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http.Filters;

using System.Diagnostics;

using DataModels;
using PhoneData;

using Newtonsoft.Json;
using System.Globalization;



namespace DataSender
{
    public class PIWebClient : IDisposable
    {
        private HttpClient _httpClient;

        private QueueConsumer _queueConsumer;

        private UserContext _userContext;

        public PIWebClient(UserContext userContext, QueueConsumer queueConsumer)
        {
            _userContext = userContext;

            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            _httpClient = new HttpClient(filter);

            _httpClient.DefaultRequestHeaders.Add("Authorization", 
                "Basic " + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", 
                userContext.Username, 
                userContext.Password))));

            _queueConsumer = queueConsumer;
        }

        public void AttachToConsumerEvent()
        {
            _queueConsumer.EventsReceived += OnEventsReceived;
        }

        public void OnEventsReceived(object sender, PhoneDataEventArgs args)
        {
            SendAsync(args.Items);
        }   

        public async void SendAsync(IList<EventItem> items)
        {
            var groups = items
                .GroupBy(i => i.Stream).Select(g => new AdhocEventsDTO { WebID = LookupWebId(g.Key), Items = ConvertToDTO(g) });

            string url = UrlBuilder.UpdateValuesAdHoc();
            Uri uri = new Uri(url);

            string adhocEventsDTO = JsonConvert.SerializeObject(groups);
            IHttpContent httpContent = new HttpStringContent(adhocEventsDTO, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, httpContent);

            Debug.WriteLine(response.StatusCode);
        }

        private IList<EventDTO> ConvertToDTO(IGrouping<StreamsEnum, EventItem> group)
        {
            return group.ToList().Select(i => new EventDTO
            {
                Timestamp = i.Timestamp.ToString("o", CultureInfo.InvariantCulture),
                Value = i.Value.ToString()
            }).ToList();

        }

        private string LookupWebId(StreamsEnum stream)
        {
            //string webID = _userContext.WebIDs[stream];
            return "P0LxDSL0sCwEC3yUrXhcG-8A5wMAAASlVQSVRFUjAwMVxBQ0NFTFg";
            //return webID;
        }

        public void Dispose()
        {
            _queueConsumer.EventsReceived -= OnEventsReceived;
            _httpClient.Dispose();
        }
    }
}
