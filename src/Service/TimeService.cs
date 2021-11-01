using Service.Contracts;

namespace Service
{
    public class TimeService : ITimeService
    {
        private readonly ITimeManager _timeManager;

        public TimeService(ITimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        public void IncreaseTime()
        {
            _timeManager.IncreaseTimeValue();
        }
    }
}
