public class Simulator : ISensorObserver
{
    public static readonly object SyncLock = new();

    private Machine[] ArrayOfMachines = [];

    public Simulator(IControlAPI controlAPI)
    {
        ArrayOfMachines = [new Machine(controlAPI) { }, new Machine(controlAPI) { }, new Machine(controlAPI) { }];
    }
    public void killTimer(Guid id)
    {
        var foundMachine = ArrayOfMachines.FirstOrDefault(m => m.GetMachineID().Equals(id));
        foundMachine?.StopTimer();
    }

    public void shutdown()
    {
        foreach (var machine in ArrayOfMachines)
        {
            killTimer(machine.GetMachineID());
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
