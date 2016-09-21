using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {        
        public MainPage()
        {
            this.InitializeComponent();

            Room1Temp.SensorId = "room1_temp";            
            Room1Hum.SensorId = "room1_hum";
            Room1Dist.SensorId = "room1_dist";

            Room2Temp.SensorId = "room2_temp";
            Room2Hum.SensorId = "room2_hum";
            Room2Dust.SensorId = "room2_dust";
        }             
    }
}