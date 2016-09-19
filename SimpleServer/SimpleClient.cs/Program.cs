using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.cs
{
    class Program
    {
        public sealed class TemperatureHum
        {
            public double Temperature { get; set; }
            public double Humidity { get; set; }

            public TemperatureHum(double temperature, double humidity)
            {
                Temperature = temperature;
                Humidity = humidity;
            }
        }

        static void Main(string[] args)
        {
            while (Console.ReadKey().KeyChar != 'C')
            {
                Do();
            }

               Console.ReadLine();

        }

        private static async void Do()
        {
            string url = @"http://localhost:8081";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));

                var random = new Random();
                while (true)
                {
                    var n = random.Next(0, 1);
                    if(n == 0)
                    {

                        var json = JsonConvert.SerializeObject(new SensorsData
                        {
                            name = "temp",
                            data = new List<Measurement>
                            {
                                new Measurement { measurement_time = DateTime.Now, unit = "c", value = "10" },
                                new Measurement { measurement_time = DateTime.Now, unit = "%", value = "11" }
                            }
                        });
                        Console.WriteLine(json);
                        //var response = await httpClient.PostAsync("/", new StringContent(json, Encoding.UTF8, "application/json"));
                        var response = await httpClient.PostAsync("/save", new StringContent(json, Encoding.UTF8, "application/json"));
                    }
                    else
                    {


                        var json = JsonConvert.SerializeObject(new SensorsData
                        {
                            name = "dist",
                            data = new List<Measurement>
                            {
                                new Measurement { measurement_time = DateTime.Now, unit = "cm", value = "111" },
                            }
                        });
                        Console.WriteLine(json);
                        //var response = await httpClient.PostAsync("/", new StringContent(json, Encoding.UTF8, "application/json"));
                        var response = await httpClient.PostAsync("/save", new StringContent(json, Encoding.UTF8, "application/json"));
                    }

                }


            }
        }
    }
}
