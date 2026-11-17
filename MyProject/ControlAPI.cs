public class ControlAPI : IControlAPI
{
    private List<ISensorObserver> list = new List<ISensorObserver>();
    private bool _emptySensor = false;
    private bool _fullPlaceSensor = false;

    public bool GetLeergutAvailable() { return _emptySensor; }
    public bool GetFullPlaceSensor() { return _fullPlaceSensor; }

    public void SetFreeLeergutAvailable(bool value)
    {
        if (_emptySensor == value)
        {
            return;
        }
        _emptySensor = value;
        list.ForEach(m => m.onEmptyPlaceSensorChanged());
    }

    public void SetFullPlaceSensor(bool value)
    {
        if (_fullPlaceSensor == value)
        {
            return;
        }

        _fullPlaceSensor = value;

        list.ForEach(m => m.onFullPlaceSensorChanged());
    }

    public void InitSimulator(ISensorObserver observer)
    {
        list.Add(observer);
    }

    public void RunControlSystem()
    {
        if (!GetLeergutAvailable())
        {
            Console.WriteLine("ControlSystem: Ordering Leergut");
            SetFreeLeergutAvailable(true);
        }

        if (GetFullPlaceSensor())
        {
            Console.WriteLine("ControlSystem: Picking up Vollgut");
            SetFullPlaceSensor(false);
        }
    }
}