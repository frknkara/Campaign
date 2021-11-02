using Data.Entities;
using Data.Repositories;
using Model.Shared;
using Moq;
using Service.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ServiceTests
{
    public class TimeManagerTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;

        public TimeManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();
        }

        [Fact]
        public void Test_GetTimeValue()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>
            {
                new SystemConfig
                {
                    Code = Constraints.TIME_SYSTEM_CONFIG_KEY,
                    Value = "10"
                }
            };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new TimeManager(_mockRepositoryFactory.Object);
            Assert.Equal(10, service.GetTimeValue());
        }

        [Fact]
        public void Test_GetTimeValue_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new TimeManager(_mockRepositoryFactory.Object);
            Exception exception = Assert.Throws<Exception>(() => service.GetTimeValue());
            Assert.Equal("System config not found.", exception.Message);
        }

        [Fact]
        public void Test_IncreaseTimeValue()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>
            {
                new SystemConfig
                {
                    Code = Constraints.TIME_SYSTEM_CONFIG_KEY,
                    Value = "1"
                }
            };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new TimeManager(_mockRepositoryFactory.Object);
            service.IncreaseTimeValue(1);
            _mockRepository.Verify(x => x.Update(
                It.IsAny<SystemConfig>(),
                It.IsAny<bool>()));
            Assert.Equal(2, service.GetTimeValue());
        }

        [Fact]
        public void Test_IncreaseTimeValue_Zero_Hour()
        {
            var service = new TimeManager(_mockRepositoryFactory.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTimeValue(0));
            Assert.Equal("Hour increment value is not valid.", exception.Message);
        }

        [Fact]
        public void Test_IncreaseTimeValue_Negative_Hour()
        {
            var service = new TimeManager(_mockRepositoryFactory.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTimeValue(-1));
            Assert.Equal("Hour increment value is not valid.", exception.Message);
        }

        [Fact]
        public void Test_IncreaseTimeValue_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new TimeManager(_mockRepositoryFactory.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTimeValue(1));
            Assert.Equal("System config not found.", exception.Message);
        }
    }
}
