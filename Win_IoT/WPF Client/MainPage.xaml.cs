using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {        
        private DistanceSensorHCSR04 _distanceSensor;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            ShowIpAddress();

            Room1Temp.SensorId = "room1_temp";            
            Room1Hum.SensorId = "room1_hum";
            Room1Dist.SensorId = "room1_dist";

            Room2Temp.SensorId = "room2_temp";
            Room2Hum.SensorId = "room2_hum";
            Room2Dust.Title = "Room 2 dust";
            Room2Dust.SensorId = "room2_dust";
        }
                
        private void ShowIpAddress()
        {
            ipAddressTbx.Text = string.Join(",  ", Helpers.Networking.GetLocalIpAddress());
        }          
    }
}