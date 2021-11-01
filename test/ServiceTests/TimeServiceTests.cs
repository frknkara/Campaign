using Moq;
using Service;
using Service.Contracts;
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
            service.IncreaseTime();
            _mockTimeManager.Verify(x => x.IncreaseTimeValue());
        }
    }
}
