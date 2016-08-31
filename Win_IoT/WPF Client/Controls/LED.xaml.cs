using Windows.UI.Xaml.Controls;
using Windows.UI;

namespace ABB.MagicMirror.Controls
{
    public sealed partial class LED : UserControl
    {
        public LED()
        {            
            InitializeComponent();
            Color = Color.FromArgb(255, 153, 0, 0);
            Off();
        }        

        /// <summary>
        /// Defines LED color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Indicates if LED is turned on.
        /// </summary>
        public bool IsTurnedOn { get; private set; }


        public void On()
        {
            LedColor.Color = Color;
            IsTurnedOn = true;
        }

        public void Off()
        {
            LedColor.Color = Color.FromArgb(255, 0, 0, 0);
            IsTurnedOn = false;
        }
    }
}
