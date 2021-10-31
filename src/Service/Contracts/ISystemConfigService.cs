namespace Service.Contracts
{
    public interface ISystemConfigService
    {
        int GetTimeValue();
        void IncreaseTimeValue();
    }
}
