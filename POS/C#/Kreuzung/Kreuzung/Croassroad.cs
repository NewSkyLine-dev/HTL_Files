namespace Kreuzung
{
    public class Crossroad
    {
        private static readonly object lockObj = new();

        public virtual void Cross(Car car)
        {
            lock (lockObj)
            {
                car.Status = "Crossing";
                Thread.Sleep(1000);
                car.Status = "Finished";
            }
        }
    }

    public class LargeCrossroad : Crossroad
    {
        private readonly object nsLock = new();
        private readonly object ewLock = new();

        public override void Cross(Car car)
        {
            object lockObject = car.Direction switch
            {
                Direction.North => nsLock,
                Direction.South => nsLock,
                Direction.East => ewLock,
                Direction.West => ewLock,
                _ => throw new InvalidOperationException("Invalid direction")
            };

            lock (lockObject)
            {
                car.Status = "Crossing";
                Thread.Sleep(1000);
                car.Status = "Finished";
            }
        }
    }

    public class TrafficLightCrossroad : Crossroad
    {
        private readonly AutoResetEvent nsEvent = new(true);
        private readonly AutoResetEvent ewEvent = new(false);

        public TrafficLightCrossroad()
        {
            Thread trafficLightThread = new(ControlTraffic);
            trafficLightThread.Start();
        }

        private void ControlTraffic()
        {
            while (true)
            {
                nsEvent.Set();
                Thread.Sleep(3000);
                nsEvent.Reset();
                Thread.Sleep(2000);

                ewEvent.Set();
                Thread.Sleep(3000);
                ewEvent.Reset();
                Thread.Sleep(2000);
            }
        }

        public override void Cross(Car car)
        {
            AutoResetEvent resetEvent = car.Direction switch
            {
                Direction.North => nsEvent,
                Direction.South => nsEvent,
                Direction.East => ewEvent,
                Direction.West => ewEvent,
                _ => throw new InvalidOperationException("Invalid direction")
            };

            resetEvent.WaitOne();
            car.Status = "Crossing";
            Thread.Sleep(1000);
            car.Status = "Finished";
        }
    }
}
