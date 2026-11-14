using static Simulator;

public class Machine
{

    private Guid machineID { get; set; } = Guid.NewGuid();

    private States State { get; set; } = States.FreeToTakeLeergut;
    private System.Timers.Timer? Timer { get; set; }

    public Machine(){
        
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
        lock (ControlAPI.SyncLock)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine($"Is Awake, what shall I do {this.machineID}?");
            switch (this.State)
            {
                case States.FreeToTakeLeergut:
                    if (!ControlAPI.getLeergutAvailable())
                    {
                        Console.WriteLine($"currently no freegut available to fill..{this.machineID}");
                        break;
                    }
                    State = States.Filling;
                    ControlAPI.setFreeLeergutAvailable(false);
                    startTimer(10, () => onFilling());
                    break;
                case States.FreeToPutVollgut:
                    if (ControlAPI.getFullPlaceSensor())
                    {
                        Console.WriteLine($"Place still occupied, cant take it machineID: {this.machineID}");
                        break;
                    }
                    State = States.FreeToTakeLeergut;
                    ControlAPI.setFullPlaceSensor(true);
                    Console.WriteLine($"Putting Vollgut machineID: {this.machineID}");
                    StartWorker();
                    Console.WriteLine($"Machine is ready to take leergut: {this.machineID}");
                    break;
                case States.Filling:
                    Console.WriteLine($"Iam filling currently machineID: {this.machineID}");
                    break;
            }
            Console.WriteLine("----------------------");
        }
    }

    void onFilling()
    {
        lock (ControlAPI.SyncLock)
        {
            State = States.FreeToPutVollgut;
            StartWorker();
        }
    }
};
