using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Campaign;
using Model.Product;
using Model.Shared;
using Moq;
using Service;
using Service.Contracts;
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
        private Mock<IProductManager> _mockProductManager;
        private Mock<ITimeManager> _mockTimeManager;
        private IMapper _mapper;
        private Guid sampleProductId;
        private ProductDto sampleProduct;

        public CampaignManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();

            _mockProductManager = new Mock<IProductManager>();
            sampleProductId = Guid.NewGuid();
            sampleProduct = new ProductDto
            {
                Id = sampleProductId,
                Code = "product",
                Price = 100,
                Stock = 50,
                CreationTime = 0
            };
            _mockProductManager.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns(sampleProduct);

            _mockTimeManager = new Mock<ITimeManager>();
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);

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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateCampaign(createCampaign));
            Assert.Equal($"The campaign with {createCampaign.Name} name has already been created.", exception.Message);
        }

        [Fact]
        public void Test_CreateCampaign()
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
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
            var campaignResult = manager.CreateCampaign(createCampaign);
            _mockProductManager.Verify(x => x.GetProductInfo(sampleProduct.Code));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            
            _mockRepository.Verify(x => x.Create(
                It.IsAny<Campaign>(),
                It.IsAny<bool>()));
            Assert.Equal(sampleProductId, campaignResult.ProductId);
            Assert.Equal(createCampaign.Name, campaignResult.Name);
            Assert.Equal(createCampaign.Duration, campaignResult.Duration);
            Assert.Equal(createCampaign.PriceManipulationLimit, campaignResult.PriceManipulationLimit);
            Assert.Equal(createCampaign.TargetSalesCount, campaignResult.TargetSalesCount);
        }

        [Fact]
        public void Test_GetCampaignInfo_Null_Name()
        {
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo(null));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo_Empty_String_Name()
        {
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetCampaignInfo(""));
            Assert.Equal("Campaign name is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetCampaignInfo_Exceeds_Max_Length_Name()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.NAME_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
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
                ProductId = sampleProductId,
                Duration = 10,
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            var list = new List<Campaign> { sampleCampaign };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Campaign>()).Returns(_mockRepository.Object);
            var manager = new CampaignManager(_mockRepositoryFactory.Object, _mockProductManager.Object, _mockTimeManager.Object, _mapper);
            var campaign = manager.GetCampaignInfo("campaign");
            Assert.Equal(sampleCampaign.Name, campaign.Name);
            Assert.Equal(sampleProductId, campaign.ProductId);
            Assert.Equal(sampleCampaign.Duration, campaign.Duration);
            Assert.Equal(sampleCampaign.PriceManipulationLimit, campaign.PriceManipulationLimit);
            Assert.Equal(sampleCampaign.TargetSalesCount, campaign.TargetSalesCount);
        }
    }
}
