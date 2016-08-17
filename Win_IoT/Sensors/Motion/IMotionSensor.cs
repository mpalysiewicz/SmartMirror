using Windows.Foundation;

namespace ABB.Sensors.Motion
{
    public interface IMotionSensor
    {
        event TypedEventHandler<IMotionSensor, string> MotionDetected;
        event TypedEventHandler<IMotionSensor, string> MotionUndetected;

        void InitGPIO();
        string Read();
    }
}