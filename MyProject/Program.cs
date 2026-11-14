class Program
{
    static void Main(string[] args)
    {
        var simulator = new Simulator();
        Console.WriteLine("Booting up...");
        ControlAPI.initSimulator(simulator);
        simulator.runMachines(); // take "Leergut" right away

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        while (!cts.IsCancellationRequested)
        {
            try { ControlAPI.runControlSystem(); }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        simulator.shutdown();
        Console.WriteLine("Simulator stopped.");
    }
}
