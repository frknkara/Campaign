using Model.Campaign;
using Moq;
using Service.Contracts;
using Service.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServiceTests
{
    public class TimeServiceTests
    {
        private Mock<ITimeManager> _mockTimeManager;
        private Mock<ICampaignManager> _mockCampaignManager;
        private Mock<IProductManager> _mockProductManager;
        private Guid _product1;
        private Guid _product2;
        private List<CampaignDto> _oldCampaigns;
        private List<CampaignDto> _currentCampaigns;

        public TimeServiceTests()
        {
            _mockTimeManager = new Mock<ITimeManager>();
            _mockCampaignManager = new Mock<ICampaignManager>();
            _mockProductManager = new Mock<IProductManager>();

            _product1 = Guid.NewGuid();
            _product2 = Guid.NewGuid();
            _oldCampaigns = new List<CampaignDto>
            {
                new CampaignDto
                {
                    Id = Guid.NewGuid(),
                    Duration = 10,
                    Name = "campaign_1",
                    ProductId = _product1,
                    ProductCode = "product1",
                    PriceManipulationLimit = 20,
                    TargetSalesCount = 100,
                    ProductInitialPrice = 100,
                    CreationTime = 0,
                    ProductPrice = 100
                },
                new CampaignDto
                {
                    Id = Guid.NewGuid(),
                    Duration = 1,
                    Name = "campaign_2",
                    ProductId = _product2,
                    ProductCode = "product2",
                    PriceManipulationLimit = 40,
                    TargetSalesCount = 50,
                    ProductInitialPrice = 200,
                    CreationTime = 0,
                    ProductPrice = 200
                },
            };
            _currentCampaigns = new List<CampaignDto>
            {
                new CampaignDto
                {
                    Id = Guid.NewGuid(),
                    Duration = 10,
                    Name = "campaign_1",
                    ProductId = _product1,
                    ProductCode = "product1",
                    PriceManipulationLimit = 20,
                    TargetSalesCount = 100,
                    ProductInitialPrice = 100,
                    CreationTime = 0,
                    ProductPrice = 100
                },
            };
        }

        [Fact]
        public void Test_IncreaseTime_Zero_Hour()
        {
            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTime(0));
            Assert.Equal("Time value is not valid.", exception.Message);
        }

        [Fact]
        public void Test_IncreaseTime_Negative_Hour()
        {
            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            Exception exception = Assert.Throws<Exception>(() => service.IncreaseTime(-1));
            Assert.Equal("Time value is not valid.", exception.Message);
        }

        [Fact]
        public void Test_IncreaseTime()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(2);

            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(0)).Returns(_oldCampaigns);
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(2)).Returns(_currentCampaigns);

            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            var result = service.IncreaseTime(2);
            _mockTimeManager.Verify(x => x.GetTimeValue());
            _mockCampaignManager.Verify(x => x.GetActiveCampaigns(0));
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(2));
            _mockCampaignManager.Verify(x => x.GetActiveCampaigns(2));

            //discount: 2hour * (100$ * 20% / 10hour) = 4 | new price: 100 - 4 = 96
            _mockProductManager.Verify(x => x.UpdateProductPrice(_product1, 96));

            //old campaigns set prices to initial prices
            _mockProductManager.Verify(x => x.UpdateProductPrice(_product2, 200));

            Assert.Equal("02:00", result);
        }

        [Fact]
        public void Test_IncreaseTime_01()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(1);

            var list = new List<CampaignDto>();
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(0)).Returns(list);
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(1)).Returns(list);
            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            var result = service.IncreaseTime(1);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(1));
            Assert.Equal("01:00", result);
        }

        [Fact]
        public void Test_IncreaseTime_13()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(1);
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(13)).Returns(14);

            var list = new List<CampaignDto>();
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(1)).Returns(list);
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(14)).Returns(list);
            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            var result = service.IncreaseTime(13);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(13));
            Assert.Equal("14:00", result);
        }

        [Fact]
        public void Test_IncreaseTime_25()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);
            _mockTimeManager.Setup(x => x.IncreaseTimeValue(It.IsAny<int>())).Returns(25);

            var list = new List<CampaignDto>();
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(0)).Returns(list);
            _mockCampaignManager.Setup(x => x.GetActiveCampaigns(25)).Returns(list);
            var service = new TimeService(_mockTimeManager.Object, _mockCampaignManager.Object, _mockProductManager.Object);
            var result = service.IncreaseTime(25);
            _mockTimeManager.Verify(x => x.IncreaseTimeValue(25));
            Assert.Equal("01:00", result);
        }
    }
}
