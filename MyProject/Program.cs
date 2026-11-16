class Program
{
    static void Main(string[] args)
    {
        //var controlAPI = new ControlAPINoImpl();
        var controlAPI = new ControlAPI();
        var simulator = new Simulator(controlAPI);
        Console.WriteLine("Booting up...");
        controlAPI.InitSimulator(simulator);
        simulator.runMachines(); // take "Leergut" right away

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        while (!cts.IsCancellationRequested)
        {
            try {
                lock(Simulator.SyncLock)
                {
                    controlAPI.RunControlSystem();
                }
                Thread.Sleep(5000);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        simulator.shutdown();
        Console.WriteLine("Simulator stopped.");
    }
}
