using ABB.Sensors.Temperature;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Web.Http;

namespace HumidityService
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

    public sealed class TempHumiditySender
    {
        private Dht22Sensor temperatureSensor;

        private GpioPin pin;

        private string url;

        public TempHumiditySender(string url)
        {
            this.url = url;

            InitGpio();
            while (true)
            {
                StartReading().Wait();
            }
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                //There is no GPIO controller on this device.
                pin = null;
                return;
            }

            pin = gpio.OpenPin(4, GpioSharingMode.Exclusive);
            if (pin == null)
            {
                //There were problems initializing the GPIO pin.;
                return;
            }
            temperatureSensor = new Dht22Sensor(pin, GpioPinDriveMode.Input);
        }

        private async Task StartReading()
        {
            try
            {
                TemperatureSensorReading temperatureReading = GetTemperatureSensorReadingResult();

                if (temperatureReading.IsValid)
                {
                    await SendHumidity(temperatureReading.Humidity);
                    await SendTemperature(temperatureReading.Temperature);
                }
            }
            catch (Exception e)
            {
            }
            await Task.Delay(30000);

        }

        private async Task SendHumidity(double humidity)
        {
            try
            {

                var data = new Data
                {
                    id = "room1_hum",
                    data = new Measurement
                    {
                        measurement_time = DateTimeOffset.Now,
                        value = humidity.ToString(),
                        unit = "%"
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

        private async Task SendTemperature(double temperature)
        {
            try
            {

                var data = new Data
                {
                    id = "room1_temp",
                    data = new Measurement
                    {
                        measurement_time = DateTimeOffset.Now,
                        value = temperature.ToString(),
                        unit = "°C"
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

        private TemperatureSensorReading GetTemperatureSensorReadingResult()
        {
            TemperatureSensorReading temperatureReading = Task.Run(GetTemperatureSensorReading).Result;
            return temperatureReading;
        }

        private async Task<TemperatureSensorReading> GetTemperatureSensorReading()
        {
            return await temperatureSensor.GetReadingAsync().AsTask();

        }
    }
}
