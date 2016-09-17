using ABB.Sensors.TemperatureWrapper;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class TemperatureAndHumidityComponent : UserControl
    {
        private TemperatureSensor temperatureSensor;

        public TemperatureAndHumidityComponent()
        {
            this.InitializeComponent();
            SetupTemperatureSensor();
        }
        
        private void SetupTemperatureSensor()
        {
            temperatureSensor = new TemperatureSensor(10000);
            if (temperatureSensor == null)
                return;
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
        }
        private async void TemperatureSensor_TemperatureRead(object sender, TemperatureReadingArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TemperatureValue.Text = e.Temperature + "°C";
                HumidiyValue.Text = e.Humidity + "%";
                SensorsDataSender.SendObjectAsJson(e);
            });
        }
    }
}
