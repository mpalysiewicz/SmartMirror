using Windows.UI.Xaml.Controls;
using Windows.UI;

namespace ABB.MagicMirror.Controls
{
    public sealed partial class LED : UserControl
    {
        public LED()
        {
            InitializeComponent();
            Off();
        }        

        public Color Color { get; set; }

        public void On()
        {
            Color = Color.FromArgb(255, 153, 0, 0);
        }

        public void Off()
        {
            Color = Color.FromArgb(255, 0, 0, 0);
        }
    }
}
