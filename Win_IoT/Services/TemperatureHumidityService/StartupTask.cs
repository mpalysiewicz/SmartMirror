using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using ABB.Sensors.TemperatureWrapper;
using SensorDataForwarder;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Abb.Services.Temperature
{
    public sealed class StartupTask : IBackgroundTask
    {
        private TemperatureHumidityReader temperatureReader;
        private const string Url = @"http://10.3.54.74:8082/";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            // 
            taskInstance.GetDeferral();           
            InitializeDataReader();
            taskInstance.Canceled += TaskInstance_Canceled;
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            throw new NotImplementedException();
        }

        private async void InitializeDataReader()
        {
            temperatureReader = new TemperatureHumidityReader(Url);
        }
    }
}
