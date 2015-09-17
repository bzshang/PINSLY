using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PIBand.Helpers;

using PIClient;
using DataOperations;

using DataModels;
using Windows.Web.Http;
using PIBand.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace PIBand.Controllers
{
    public class PhoneDataController
    {
        private SessionContext _sessionCtx;

        private DataProcessor _dataProcessor;

        private IList<PIWebClient> _piWebClients;

        private EventFrameInfo _eventFrameInfo;
        private PIWebClient _efClient;

        private string _efWebId;

        public PhoneDataController()
        {
            _sessionCtx = SessionContextProvider.GetSessionContext();
            _dataProcessor = new DataProcessor(_sessionCtx.DataContext);

            _piWebClients = _dataProcessor.Consumers.Select(consumer => new PIWebClient(_sessionCtx.UserContext, consumer)).ToList();

            
            _efClient = new PIWebClient(_sessionCtx.UserContext, null);
        }

        public async Task InitializeAsync()
        {
            foreach (PIWebClient client in _piWebClients)
            {
                client.AttachToConsumerEvent();
            }

            _eventFrameInfo = new EventFrameInfo();
            _eventFrameInfo.StartTime = DateTime.Now;
            _eventFrameInfo.Name = _sessionCtx.UserContext.Username + " - " + _eventFrameInfo.StartTime.ToUniversalTime();

            string afDbWebId = await GetAFDatabaseWebID();

            EventFrameDTO efDTO = new EventFrameDTO
            {
                Name = _eventFrameInfo.Name,
                StartTime = _eventFrameInfo.StartTime.ToString("o", CultureInfo.InvariantCulture),
                EndTime = "",
                TemplateName = "MobileEF"
            };

            string jsonString = JsonConvert.SerializeObject(efDTO);
            HttpResponseMessage response = await _efClient.OpenEventFrame(afDbWebId, jsonString);

            _efWebId = GetEFWebId(response);

            await SetEFAttributes();

            _dataProcessor.Start();
        }

        private async Task<string> GetAFDatabaseWebID()
        {
            string webId = AppSettingsManager.Instance.LocalSettings.Containers["ServerSettings"].GetValueOrDefault("WebId_AFDatabase", "");

            if (!string.IsNullOrEmpty(webId)) return webId;

            HttpResponseMessage responseGetDatabase = await _efClient.GetDatabaseByPath(@"\\jupiter001\pifitness2.0");
            dynamic valueResult;
            using (StreamReader sr = new StreamReader((await responseGetDatabase.Content.ReadAsInputStreamAsync()).AsStreamForRead()))
            {
                using (JsonTextReader jReader = new JsonTextReader(sr))
                {
                    valueResult = JObject.ReadFrom(jReader);
                }
            }

            string newWebId = valueResult["WebId"];

            if (!string.IsNullOrEmpty(newWebId)) AppSettingsManager.Instance.LocalSettings.Containers["ServerSettings"].AddOrUpdateValue("WebId_AFDatabase", newWebId);

            return newWebId;
        }

        public async Task Close()
        {
            await _dataProcessor.Stop();
            _eventFrameInfo.EndTime = DateTime.Now;

            var efDTO = new 
            {
                EndTime = _eventFrameInfo.EndTime.ToString("o", CultureInfo.InvariantCulture),
            };

            string jsonString = JsonConvert.SerializeObject(efDTO);

            HttpResponseMessage response = await _efClient.CloseEventFrame(_efWebId, jsonString);

            foreach (PIWebClient client in _piWebClients)
            {
                client.Dispose();
            }

            
        }

        private string GetEFWebId(HttpResponseMessage response)
        {
            return response.Headers.Location.Segments.Last();
        }

        private async Task SetEFAttributes()
        {
            HttpResponseMessage responseGetEFAttributes = await _efClient.GetEFAttributes(_efWebId);

            dynamic valueResult;
            if (responseGetEFAttributes.StatusCode == HttpStatusCode.Ok)
            {
                using (StreamReader sr = new StreamReader((await responseGetEFAttributes.Content.ReadAsInputStreamAsync()).AsStreamForRead()))
                {
                    using (JsonTextReader jReader = new JsonTextReader(sr))
                    {
                        valueResult = JObject.ReadFrom(jReader);
                    }
                }
                IList<AttributeDTO> attributes = valueResult["Items"].ToObject<List<AttributeDTO>>();
                foreach (AttributeDTO attr in attributes)
                {
                    StringBuilder sb = new StringBuilder();

                    string userName = _sessionCtx.UserContext.Username;

                    string last = attr.ConfigString.Split(new char[] { '\\' }).Last();
                    string pointName = attr.ConfigString.Replace(@"%Element%", userName);

                    //foreach (string s in attr.ConfigString.Split('\\'))
                    //{
                    //    if (s.Equals(last))
                    //    {
                    //        string ptName = userName + s;
                    //        sb.Append('\\').Append(ptName);
                    //    }
                    //    else
                    //    {
                    //        sb.Append('\\').Append(s);
                    //    }
                    //}

                    //pointName = sb.ToString();

                    var config = new
                    {
                        Name = attr.Name,
                        ConfigString = pointName
                    };

                    string jsonString = JsonConvert.SerializeObject(config);
                    HttpResponseMessage responseUpdateAttribute = await _efClient.UpdateAttribute(attr.WebId, jsonString);
                }


            }



        }

    }
}
