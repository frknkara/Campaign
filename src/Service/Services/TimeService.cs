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

        public string IncreaseTime(int hour)
        {
            var updatedTime = _timeManager.IncreaseTimeValue(hour);
            //TODO: inject campaign service, product service. get products in campaign and set prices by campaign discount
            var time = updatedTime % 24;
            return time.ToString("D2") + ":00";
        }
    }
}
