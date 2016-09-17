using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ABB.MagicMirror
{
    public static class SensorsDataSender
    {
        private const string url = @"http://192.168.7.131:3000";
        public static async void SendObjectAsJson(object objectToSend)
        {
            if (objectToSend == null)
            {
                return;
            }
            try
            {
                using (var httpClient = GetClient())
                {
                    var json = JsonConvert.SerializeObject(objectToSend);

                    var response = await httpClient.PostAsync("/", new StringContent(json, Encoding.UTF8, "application/json"));
                }
            }
            catch (Exception e)
            {

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
