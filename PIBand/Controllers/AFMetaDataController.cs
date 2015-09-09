using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataSender;
using PhoneData;
using Windows.Web.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PIBand.Controllers
{
    public class AFMetaDataController
    {
        private PIWebClient _piWebClient;

        private SessionContext _sessionContext;

        public AFMetaDataController()
        {
            _sessionContext = SessionContextProvider.GetSessionContext();
            _piWebClient = new PIWebClient(_sessionContext.UserContext, null);
        }
       

        public async Task BuildUserAssets()
        {
            bool existsAF = await CheckAF();

            if (existsAF)
            {
                bool existsPI = await CheckPI();
                if (existsPI)
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

        private async Task<bool> CheckAF()
        {
            HttpResponseMessage responseCheckExisting = await CheckExistingAF();

            if (responseCheckExisting.StatusCode == HttpStatusCode.NotFound)
            {
                HttpResponseMessage responseGetUserContainer = await _piWebClient.GetElementByPath(@"\\jupiter001\pifitness2.0\users");
                dynamic valueResult;
                using (StreamReader sr = new StreamReader((await responseGetUserContainer.Content.ReadAsInputStreamAsync()).AsStreamForRead()))
                {
                    using (JsonTextReader jReader = new JsonTextReader(sr))
                    {
                        valueResult = JObject.ReadFrom(jReader);
                    }
                }
                string webID = valueResult["WebId"];
                var jsonObject = new
                {
                    Name = _sessionContext.UserContext.Username,
                    TemplateName = "MobileData"
                };

                string jsonString = JsonConvert.SerializeObject(jsonObject);

                HttpResponseMessage responseCreateUserElement = await _piWebClient.CreateElement(webID, jsonString);

                if (responseCreateUserElement.StatusCode != HttpStatusCode.Created)
                {
                    // TODO
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (responseCheckExisting.StatusCode == HttpStatusCode.Ok)
            {
                return true;
            }
 
            return false;
          
        }

        private async Task<bool> CheckPI()
        {
            string webID = await GetServerWebID(); 

            string nameFilter = string.Format("{0}.MobileData*", _sessionContext.UserContext.Username);

            HttpResponseMessage responseGetPoints = await _piWebClient.GetPoints(webID, nameFilter);

            bool pointsCreationStatus = false;

            dynamic valueResult;
            if (responseGetPoints.StatusCode == HttpStatusCode.Ok)
            {
                using (StreamReader sr = new StreamReader((await responseGetPoints.Content.ReadAsInputStreamAsync()).AsStreamForRead()))
                {
                    using (JsonTextReader jReader = new JsonTextReader(sr))
                    {
                        valueResult = JObject.ReadFrom(jReader);
                    }
                }
                string[] points = valueResult["Items"];




            }
            else if (responseGetPoints.StatusCode == HttpStatusCode.NotFound)
            {
                pointsCreationStatus = await CreatePointsAsync(webID);
            }

            return pointsCreationStatus;
        }

        private async Task<string> GetServerWebID()
        {
            HttpResponseMessage responseGetServer = await _piWebClient.GetServerByPath(@"\\jupiter001");
            dynamic valueResult;
            using (StreamReader sr = new StreamReader((await responseGetServer.Content.ReadAsInputStreamAsync()).AsStreamForRead()))
            {
                using (JsonTextReader jReader = new JsonTextReader(sr))
                {
                    valueResult = JObject.ReadFrom(jReader);
                }
            }

            return valueResult["WebId"];
        }

        private async Task<bool> CreatePointsAsync(string webID)
        {
            string[] pointTemplateNames =
            {
                "PhoneAccelerometerX",
                "PhoneAccelerometerY",
                "PhoneAccelerometerZ",
                "Geoposition_Latitude",
                "Geoposition_Longitude"
            };

            var pointNames = pointTemplateNames.Select(i => string.Format("{0}.MobileData.{1}", _sessionContext.UserContext.Username, i));

            Task<bool>[] tasks = pointNames.Select(async i => await CreatePointAsync(webID, i)).ToArray();

            bool[] results = await Task.WhenAll(tasks);

            return (results.Contains(false)) ? false : true;
        }

        private async Task<bool> CreatePointAsync(string webID, string pointName)
        {
            var jsonObject = new
            {
                Name = pointName,
                PointClass = "classic",
                PointType = "Float32"
            };
            string jsonString = JsonConvert.SerializeObject(jsonObject);

            HttpResponseMessage responseCreatePoint = await _piWebClient.CreatePoint(webID, jsonString);

            if (responseCreatePoint.StatusCode == HttpStatusCode.Created || responseCreatePoint.Content.ToString().Contains("-10550"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<HttpResponseMessage> CheckExistingAF()
        { 
            HttpResponseMessage response = await _piWebClient.GetElementByPath(@"\\jupiter001\pifitness2.0\users\" + _sessionContext.UserContext.Username);

            return response;
        }

        public void Close()
        {
            _piWebClient.Dispose();
        }

    }
}
