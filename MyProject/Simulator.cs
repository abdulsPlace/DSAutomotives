public class Simulator : ISensor
    {
        private Machine[] ArrayOfMachines = [
            new Machine() {}, new Machine() { }, new Machine() {}
            ];

        public void killTimer(Guid id)
        {
            lock (ControlAPI.SyncLock)
            {
                var foundMachine = ArrayOfMachines.FirstOrDefault(m => m.GetMachineID().Equals(id));
                foundMachine?.StopTimer();
            }
        }

        public void shutdown()
        {
            lock (ControlAPI.SyncLock)
            {
                foreach (var machine in ArrayOfMachines)
                {
                    killTimer(machine.GetMachineID());
                }
            }
        }

    public void runMachines()
        {
            foreach (var machine in ArrayOfMachines)
            {
                machine.StartWorker();
            }

        }

        public void onEmptyPlaceSensorChanged()
        {
            this.runMachines();
        }

        public void onFullPlaceSensorChanged()
        {
            this.runMachines();
        }

        public enum States
        {
            FreeToTakeLeergut,
            Filling,
            FreeToPutVollgut
        }

    }
