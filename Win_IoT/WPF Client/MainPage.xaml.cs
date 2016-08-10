using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MotionSensorService;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MotionSensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private DispatcherTimer timer;
        private MotionSensorService.MotionSensor motionSensor;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            motionSensor = new MotionSensorService.MotionSensor();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();


        }

        private void Timer_Tick(object sender, object e)
        {
            var value = motionSensor.Read();
            if (value == "High")
                GpioStatus.Items.Add("Motion detected at " + DateTime.Now);
        }

        private void MotionSensor_MotionDetected(MotionSensorService.MotionSensor sender, string e)
        {
            //GpioStatus.Text += "\r\nMotion detected at " + DateTime.Now;
        }
        
    }
}
