public static class ControlAPI
    {
        public static readonly object SyncLock = new();

        static List<ISensor> list = new List<ISensor>();
        static bool _emptySensor = false;
        static bool _fullPlaceSensor = false;

        public static bool getLeergutAvailable() { lock (SyncLock) { return _emptySensor; } }
        public static bool getFullPlaceSensor() { lock (SyncLock) { return _fullPlaceSensor; } }

        public static void setFreeLeergutAvailable(bool value)
        {
            lock (SyncLock)
            {
                if (_emptySensor == value)
                {
                    return;
                }
                _emptySensor = value;
                list.ForEach(m => m.onEmptyPlaceSensorChanged());
            }
        }

        public static void setFullPlaceSensor(bool value)
        {
            lock (SyncLock)
            {
                if (_fullPlaceSensor == value)
                {
                    return;
                }

                _fullPlaceSensor = value;

                list.ForEach(m => m.onFullPlaceSensorChanged());
            }
        }

        public static void initSimulator(Simulator simulator)
        {
            list.Add(simulator);
        }

        public static void runControlSystem()
        {
            lock (SyncLock)
            {
                if (!getLeergutAvailable()) // no freegut Available now it gets available by delivering.
                {
                    setFreeLeergutAvailable(true);
                }

                if (getFullPlaceSensor())
                { // full => emptying it
                    setFullPlaceSensor(false);
                }
            }

        }

    }

