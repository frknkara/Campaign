using Moq;
using Service.Contracts;
using Service.Services;
using Xunit;

namespace ServiceTests
{
    public class TimeServiceTests
    {
        private Mock<ITimeManager> _mockTimeManager;

        public TimeServiceTests()
        {
            _mockTimeManager = new Mock<ITimeManager>();
        }

        [Fact]
        public void Test_IncreaseTime()
        {
            var service = new TimeService(_mockTimeManager.Object);
            service.IncreaseTime(1);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(1));
        }
    }
}