
using Windows.Storage;
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
            var dialog = new ContentDialog();
            var settings = new Settings();
            dialog.Content = settings;
            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick; 
            dialog.SecondaryButtonText = "Cancel";
            var dialogResult = dialog.ShowAsync();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var settings = sender.Content as Settings;
            if(settings == null)
            {
                return;
            }

            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["SensorServiceUrl"] = settings.SensorsServiceUrl;
        }
    }
}