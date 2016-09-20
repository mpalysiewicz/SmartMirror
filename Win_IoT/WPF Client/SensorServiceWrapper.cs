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
        
        public async static Task<JObject> DownloadLatestMeasurementById(string url, string id)
        {
            try
            {
                using (var httpClient = GetClient(url))
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
