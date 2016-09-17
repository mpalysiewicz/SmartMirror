namespace ABB.Sensors.Distance
{
    public class DistanceReading
    {
        public double DistanceInCm { get; set; }

        public DistanceReading(double distanceInCm)
        {
            DistanceInCm = distanceInCm;
        }
    }
}
