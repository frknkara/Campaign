using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Campaign;
using Model.Shared;
using Moq;
using Service;
using Service.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ServiceTests
{
    public class CampaignManagerTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;
        private IMapper _mapper;

        public CampaignManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();

            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void Test_CreateCampaign_Null_Name()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = null,
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Empty_String_Name()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Exceeds_Max_Length_Name()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.NAME_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var createCampaign = new CreateCampaignDto
            {
                Name = invalidLengthString,
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal($"Max length of campaign name should be {Constraints.NAME_COLUMN_MAX_LENGTH}.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Zero_Duration()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 0,
                PriceManipulationLimit = 20,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Duration is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Negative_Duration()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = -1,
                PriceManipulationLimit = 20,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Duration is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Zero_PriceManipulationLimit()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 0,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Price manipulation limit is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Negative_PriceManipulationLimit()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = -1,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Price manipulation limit is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Hundred_PriceManipulationLimit()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 100,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Price manipulation limit is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Greater_Than_Hundred_PriceManipulationLimit()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 101,
                TargetSalesCount = 50
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Price manipulation limit is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Negative_TargetSalesCount()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = -1
            };
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal("Target sales count is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign_Same_Campaign_Name_Exist()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            var _mockRepository = new Mock<IRepository<Campaign>>();
            var list = new List<Campaign>
            {
                new Campaign
                {
                    Name = "campaign",
                    ProductId = Guid.NewGuid(),
                    Duration = 10,
                    PriceManipulationLimit = 20,
                    TargetSalesCount = 100
                }
            };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal($"The campaign with {createCampaign.Name} name has already been created.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign()
        {
            var createCampaign = new CreateCampaignDto
            {
                Name = "campaign",
                ProductId = Guid.NewGuid(),
                ProductCode = "product",
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            var _mockRepository = new Mock<IRepository<Campaign>>();
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            var campaignResult = manager.CreateCampaign(createCampaign);
            
            _mockRepository.Verify(x => x.Create(
                It.IsAny<Campaign>(),
                It.IsAny<bool>()));
            Assert.Equal(createCampaign.ProductId, campaignResult.ProductId);
            Assert.Equal(createCampaign.Name, campaignResult.Name);
            Assert.Equal(createCampaign.Duration, campaignResult.Duration);
            Assert.Equal(createCampaign.PriceManipulationLimit, campaignResult.PriceManipulationLimit);
            Assert.Equal(createCampaign.TargetSalesCount, campaignResult.TargetSalesCount);
        }

        [Fact]
        public void Test_GetCampaignInfo_Null_Name()
        {
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo(null));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo_Empty_String_Name()
        {
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo(""));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo_Exceeds_Max_Length_Name()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.NAME_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo(invalidLengthString));
            Assert.Equal($"Max length of campaign name should be {Constraints.NAME_COLUMN_MAX_LENGTH}.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo_Campaign_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<Campaign>>();
            var list = new List<Campaign>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo("product"));
            Assert.Equal("Campaign not found.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo()
        {
            var _mockRepository = new Mock<IRepository<Campaign>>();
            var sampleCampaign = new Campaign
            {
                Name = "campaign",
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            var list = new List<Campaign> { sampleCampaign };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            var campaign = manager.GetCampaignInfo("campaign");
            Assert.Equal(sampleCampaign.Name, campaign.Name);
            Assert.Equal(sampleCampaign.ProductId, campaign.ProductId);
            Assert.Equal(sampleCampaign.Duration, campaign.Duration);
            Assert.Equal(sampleCampaign.PriceManipulationLimit, campaign.PriceManipulationLimit);
            Assert.Equal(sampleCampaign.TargetSalesCount, campaign.TargetSalesCount);
        }

        [Fact]
        public void Test_GetCampaignOrders()
        {
            var sampleCampaign = new CampaignDto
            {
                Name = "campaign",
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 100,
                CreationTime = 0
            };

            var _mockOrderRepository = new Mock<IRepository<Order>>();
            var sampleOrders = new List<Order>
            {
                new Order
                {
                    ProductId = sampleCampaign.ProductId,
                    Quantity = 3,
                    CreationTime = 0
                },
                new Order
                {
                    ProductId = sampleCampaign.ProductId,
                    Quantity = 4,
                    CreationTime = 1
                },
                new Order
                {
                    ProductId = sampleCampaign.ProductId,
                    Quantity = 5,
                    CreationTime = 10
                }
            };
            _mockOrderRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Order, bool>>>())).Returns(sampleOrders.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Order>()).Returns(_mockOrderRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mapper);
            var result = manager.GetCampaignOrders(sampleCampaign);
            Assert.Equal(3, result.Count);
            Assert.True(result.All(x => x.ProductId == sampleCampaign.ProductId));
            Assert.Equal(new List<int> { 3, 4, 5 }, result.Select(x => x.Quantity).ToList());
        }
    }
}
