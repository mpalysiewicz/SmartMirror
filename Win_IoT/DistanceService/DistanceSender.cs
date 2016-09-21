using ABB.Sensors.Distance;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace DistanceService
{
    public sealed class Data
    {
        public string id { get; set; }
        public Measurement data { get; set; }
    }

    public sealed class Measurement
    {
        public DateTimeOffset measurement_time { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }
    public sealed class DistanceSender
    {
        private string url;
        private DistanceSensorHCSR04 distanceSensor;

        public DistanceSender(string url)
        {
            this.url = url;

            distanceSensor = new DistanceSensorHCSR04();
            distanceSensor.InitGPIO();

            while (true)
            {
                StartReading().Wait();
            }
        }
        private async Task SendDistance(double distanceInCm)
        {
            try
            {
                var data = new Data
                {
                    id = "room1_dist",
                    data = new Measurement
                    {
                        measurement_time = DateTimeOffset.Now,
                        value = distanceInCm.ToString(),
                        unit = "cm"
                    }
                };
                var json = JsonConvert.SerializeObject(data);

                using (var httpClient = new HttpClient())
                {
                    var msg = new HttpRequestMessage(HttpMethod.Post, new Uri(url + "/save"));
                    msg.Content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    msg.Content.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/json");
                    
                    var response = await httpClient.SendRequestAsync(msg);
                }
            }
            catch (Exception e)
            {

            }            
        }

        private async Task StartReading()
        {
            var distanceReading = distanceSensor.Read();
            if (distanceReading == null)
            {
                return;
            }
            await SendDistance(distanceReading.DistanceInCm);
            await Task.Delay(5000);           

        }
    }


}