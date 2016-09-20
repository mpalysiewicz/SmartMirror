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
        }
                
        private void ShowIpAddress()
        {
            ipAddressTbx.Text = string.Join(",  ", Helpers.Networking.GetLocalIpAddress());
        }          
    }
}