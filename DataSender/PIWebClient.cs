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
using DataOperations;
using DataSender;

using Newtonsoft.Json;
using System.Globalization;



namespace PIClient
{
    public class PIWebClient : IDisposable
    {
        private HttpClient _httpClient;

        private QueueConsumer _queueConsumer;

        private UserContext _userContext;

        private IUpdateValues _updateValuesService;

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

            _updateValuesService = new UpdateValuesAdhocService();
        }

        public void AttachToConsumerEvent()
        {
            _queueConsumer.EventsReceived += OnEventsReceived;
        }

        public void OnEventsReceived(object sender, DataEventArgs args)
        {
            SendAsync(args.Items);
        }   

        public async void SendAsync(IList<EventItem> items)
        {
            Uri uri = _updateValuesService.GetUri();
            IHttpContent httpContent = _updateValuesService.GetHttpContent(_userContext, items);

            HttpResponseMessage response = await _httpClient.PostAsync(uri, httpContent);

            Debug.WriteLine(response.StatusCode);
        }

        public async Task<HttpResponseMessage> GetElementByPath(string path)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/elements?path=" + path;
            Uri uri = new Uri(url);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> CreateElement(string webID, string jsonString)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/elements/" + webID + @"/elements";
            Uri uri = new Uri(url);
            IHttpContent httpContent = new HttpStringContent(jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(uri, httpContent);

            return response;
        }

        public async Task<HttpResponseMessage> CreatePoint(string webID, string jsonString)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/dataservers/" + webID + @"/points";
            Uri uri = new Uri(url);
            IHttpContent httpContent = new HttpStringContent(jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(uri, httpContent);

            return response;
        }

        public async Task<HttpResponseMessage> GetServerByPath(string path)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/dataservers?path=" + path;
            Uri uri = new Uri(url);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> GetDatabaseByPath(string path)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/assetdatabases?path=" + path;
            Uri uri = new Uri(url);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> GetPoints(string webID, string nameFilter)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/dataservers/" + webID + "/points?nameFilter=" + nameFilter; 
            Uri uri = new Uri(url);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> OpenEventFrame(string webId, string jsonString)
        {        
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/assetdatabases/" + webId + "/eventframes";
            Uri uri = new Uri(url);

            IHttpContent httpContent = new HttpStringContent(jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(uri, httpContent);

            return response;
        }

        public async Task<HttpResponseMessage> CloseEventFrame(string webId, string jsonString)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/eventframes/" + webId;
            Uri uri = new Uri(url);

            IHttpContent httpContent = new HttpStringContent(jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync(uri, httpContent);

            return response;
        }

        public async Task<HttpResponseMessage> GetEFAttributes(string webId)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/eventframes/" + webId + "/attributes";
            Uri uri = new Uri(url);

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> UpdateAttribute(string webId, string jsonString)
        {
            string url = @"https://osiproghack01.cloudapp.net/piwebapi/attributes/" + webId;
            Uri uri = new Uri(url);

            IHttpContent httpContent = new HttpStringContent(jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync(uri, httpContent);

            return response;
        }

        public void Dispose()
        {
            if (_queueConsumer != null) _queueConsumer.EventsReceived -= OnEventsReceived;
            _httpClient.Dispose();
        }
    }
}
