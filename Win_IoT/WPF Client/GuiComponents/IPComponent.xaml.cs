using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class IPComponent : UserControl
    {
        public IPComponent()
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
