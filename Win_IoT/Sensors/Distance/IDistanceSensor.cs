using Windows.Foundation;

namespace ABB.Sensors.Distance
{
    public interface IDistanceSensor
    {
        event TypedEventHandler<IDistanceSensor, string> DistanceChanged;

        void InitGPIO();
        string Read();
    }
}