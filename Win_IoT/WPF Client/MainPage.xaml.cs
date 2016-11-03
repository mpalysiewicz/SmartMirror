using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}