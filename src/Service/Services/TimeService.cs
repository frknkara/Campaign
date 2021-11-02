using Service.Contracts;

namespace Service.Services
{
    public class TimeService : ITimeService
    {
        private readonly ITimeManager _timeManager;

        public TimeService(ITimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        public void IncreaseTime(int hour)
        {
            _timeManager.IncreaseTimeValue(hour);
            //TODO: inject campaign service, product service. get products in campaign and set prices by campaign discount
        }
    }
}
