public interface IControlAPI
{
    public bool GetLeergutAvailable();
    public void SetFreeLeergutAvailable(bool value);
    public bool GetFullPlaceSensor();
    public void SetFullPlaceSensor(bool value);
    public void InitSimulator(ISensorObserver simulator);
    public void RunControlSystem();
}