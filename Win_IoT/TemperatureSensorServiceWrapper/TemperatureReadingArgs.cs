namespace TemperatureSensorServiceWrapper
{
    public sealed class TemperatureReadingArgs
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }

        public TemperatureReadingArgs(double temperature, double humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }
    }
}
