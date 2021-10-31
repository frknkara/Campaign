using Data.Entities;
using Data.Repositories;
using Model.Shared;
using Moq;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ServiceTests
{
    public class SystemConfigServiceTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;

        public SystemConfigServiceTests()
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
            var service = new SystemConfigService(_mockRepositoryFactory.Object);
            Assert.Equal(10, service.GetTimeValue());
        }

        [Fact]
        public void Test_GetTimeValue_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new SystemConfigService(_mockRepositoryFactory.Object);
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
            var service = new SystemConfigService(_mockRepositoryFactory.Object);
            service.IncreaseTimeValue();
            _mockRepository.Verify(x => x.Update(
                It.IsAny<SystemConfig>(),
                It.IsAny<bool>()));
            Assert.Equal(2, service.GetTimeValue());
        }

        [Fact]
        public void Test_IncreaseTimeValue_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<SystemConfig>>();
            var list = new List<SystemConfig>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<SystemConfig, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<SystemConfig>()).Returns(_mockRepository.Object);
            var service = new SystemConfigService(_mockRepositoryFactory.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTimeValue());
            Assert.Equal("System config not found.", exception.Message);
        }
    }
}
