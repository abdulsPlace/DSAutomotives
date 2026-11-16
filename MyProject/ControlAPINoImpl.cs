public class ControlAPINoImpl : IControlAPI
{
    public bool GetFullPlaceSensor()
    {
        Console.WriteLine("GetFullPlaceSensor: not implemented yet");
        return false;
    }

    public bool GetLeergutAvailable()
    {
        Console.WriteLine("GetLeergutAvailable: not implemented yet");
        return false;
    }

    public void InitSimulator(ISensor simulator)
    {
        Console.WriteLine("InitSimulator: not implemented yet");
    }

    public void RunControlSystem()
    {
        Console.WriteLine("RunControlSystem: not implemented yet");
    }

    public void SetFreeLeergutAvailable(bool value)
    {
        Console.WriteLine($"SetFreeLeergutAvailable({value}): not implemented yet");
    }

    public void SetFullPlaceSensor(bool value)
    {
        Console.WriteLine($"SetFullPlaceSensor({value}): not implemented yet");
    }
}
