using static Simulator;

public class Machine
{
    private Guid machineID { get; set; } = Guid.NewGuid();
    private States State { get; set; } = States.FreeToTakeLeergut;
    private System.Timers.Timer? Timer { get; set; }

    private IControlAPI controlAPI;

    public Machine(IControlAPI controlAPI)
    {
        this.controlAPI = controlAPI;
    }

    public void startTimer(int seconds, Action callback)
    {
        this.Timer = new System.Timers.Timer(TimeSpan.FromSeconds(seconds));
        this.Timer.AutoReset = false;
        this.Timer.Elapsed += (s, e) =>
        {
            try { callback(); }
            catch (Exception ex) { Console.WriteLine($"[{machineID}] timer callback failed: {ex}"); }
        };
        this.Timer.Start();
    }

    public Guid GetMachineID()
    {
        return this.machineID;
    }

    public void StopTimer()
    {
        this.Timer?.Stop();
    }

    public void StartWorker()
    {
        lock (Simulator.SyncLock)
        {
            Console.WriteLine($"-------------- Start {this.machineID} ------------");
            switch (this.State)
            {
                case States.FreeToTakeLeergut:
                    if (!this.controlAPI.GetLeergutAvailable())
                    {
                        Console.WriteLine($"currently no freegut available to fill..{this.machineID}");
                        break;
                    }
                    State = States.Filling;
                    this.controlAPI.SetFreeLeergutAvailable(false);
                    startTimer(10, () => onFilling());
                    break;
                case States.FreeToPutVollgut:
                    if (this.controlAPI.GetFullPlaceSensor())
                    {
                        Console.WriteLine($"Place still occupied, cant take it machineID: {this.machineID}");
                        break;
                    }
                    State = States.FreeToTakeLeergut;
                    this.controlAPI.SetFullPlaceSensor(true);
                    Console.WriteLine($"Putting Vollgut machineID: {this.machineID}");
                    StartWorker();
                    Console.WriteLine($"Machine is ready to take leergut: {this.machineID}");
                    break;
                case States.Filling:
                    Console.WriteLine($"Iam filling currently machineID: {this.machineID}");
                    break;
            }
            Console.WriteLine($"-------------- END {this.machineID} ------------");
            Console.WriteLine("");
        }
    }

    void onFilling()
    {
        State = States.FreeToPutVollgut;
        StartWorker();
    }
};
