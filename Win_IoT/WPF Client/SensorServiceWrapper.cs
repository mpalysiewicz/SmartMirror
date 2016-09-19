using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ABB.MagicMirror
{
    public static class SensorServiceWrapper
    {
        private const string url = @"http://192.168.7.131:3000";
        
        public async static Task<JObject> DownloadLatestMeasurementById(string id)
        {
            try
            {
                using (var httpClient = GetClient())
                {
                    var response = httpClient.GetAsync(string.Format("/lastValue/{0}", id)).Result;
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

        private static HttpClient GetClient()
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
