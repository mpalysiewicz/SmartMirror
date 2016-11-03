using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace ABB.MagicMirror
{
    public static class SensorServiceWrapper
    {
        //private const string url = @"http://10.3.54.74:8082";

        public static string Url
        {
            get
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                var value = localSettings.Values["SensorServiceUrl"] as string;
                return value;
            }
        }

        public async static Task<JObject> DownloadLatestMeasurementById(string id)
        {
            try
            {
                using (var httpClient = GetClient(Url))
                {
                    var response = httpClient.GetAsync(string.Format("/{0}/lastValue", id)).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JObject.Parse(responseBody);
                }
            }
            catch (Exception e)
            {

                return null;
            }            
        }

        private static HttpClient GetClient(string url)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
