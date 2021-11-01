namespace Service.Contracts
{
    public interface ITimeManager
    {
        int GetTimeValue();
        void IncreaseTimeValue(int hour);
    }
}
