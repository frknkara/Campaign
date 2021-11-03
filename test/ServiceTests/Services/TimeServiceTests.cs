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
        public void Test_IncreaseTime_01()
        {
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(1);
            var service = new TimeService(_mockTimeManager.Object);
            var result = service.IncreaseTime(1);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(1));
            Assert.Equal("01:00", result);
        }

        [Fact]
        public void Test_IncreaseTime_13()
        {
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(13);
            var service = new TimeService(_mockTimeManager.Object);
            var result = service.IncreaseTime(1);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(1));
            Assert.Equal("13:00", result);
        }

        [Fact]
        public void Test_IncreaseTime_25()
        {
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(25);
            var service = new TimeService(_mockTimeManager.Object);
            var result = service.IncreaseTime(1);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(1));
            Assert.Equal("01:00", result);
        }
    }
}
