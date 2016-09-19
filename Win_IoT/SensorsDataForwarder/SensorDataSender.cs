using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace SensorDataForwarder
{
    public class SensorDataSender
    {
        private readonly string url;// = @"http://192.168.7.131:3000";

        public SensorDataSender(string url)
        {
            this.url = url;
        }
        public async void SendObjectAsJson(object objectToSend)
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

        private HttpClient GetClient()
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
