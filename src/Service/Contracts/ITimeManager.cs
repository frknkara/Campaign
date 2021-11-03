namespace Service.Contracts
{
    public interface ITimeManager
    {
        int GetTimeValue();
        int IncreaseTimeValue(int hour);
    }
}
