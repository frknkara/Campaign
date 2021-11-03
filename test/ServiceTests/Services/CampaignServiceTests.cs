using Model.Campaign;
using Model.Order;
using Model.Product;
using Moq;
using Service.Contracts;
using Service.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServiceTests
{
    public class CampaignServiceTests
    {
        private Mock<ICampaignManager> _mockCampaignManager;
        private Mock<IProductManager> _mockProductManager;
        private Mock<ITimeManager> _mockTimeManager;
        private Guid sampleProductId;
        private ProductDto sampleProduct;

        public CampaignServiceTests()
        {
            _mockCampaignManager = new Mock<ICampaignManager>();
            _mockProductManager = new Mock<IProductManager>();
            _mockTimeManager = new Mock<ITimeManager>();

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
        }

        [Fact]
        public void Test_CreateCampaign()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);
            var createCampaign = new CreateCampaignDto
            {
                ProductCode = "product",
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            var campaign = new CampaignDto
            {
                ProductId = sampleProductId,
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            _mockCampaignManager.Setup(x => x.CreateCampaign(It.IsAny<CreateCampaignDto>())).Returns(campaign);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.CreateCampaign(createCampaign.Name, createCampaign.ProductCode, createCampaign.Duration, createCampaign.PriceManipulationLimit, createCampaign.TargetSalesCount);
            _mockProductManager.Verify(x => x.GetProductInfo(createCampaign.ProductCode));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            Assert.Equal($"Campaign created; name {createCampaign.Name}, product {createCampaign.ProductCode}, duration {createCampaign.Duration}, limit {createCampaign.PriceManipulationLimit}, target sales count {createCampaign.TargetSalesCount}", result);
        }

        [Fact]
        public void Test_GetCampaignInfo_Active()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(1);
            var campaign = new CampaignDto
            {
                ProductId = sampleProductId,
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100,
                CreationTime = 0
            };
            _mockCampaignManager.Setup(x => x.GetCampaignInfo(It.IsAny<string>())).Returns(campaign);
            var sampleOrders = new List<OrderDto>
            {
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 3,
                    CreationTime = 0
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 4,
                    CreationTime = 1
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 5,
                    CreationTime = 5
                }
            };
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<string>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            Assert.Equal($"Campaign {campaign.Name} info; Status Active, Target Sales {campaign.TargetSalesCount}, Total Sales {12}, Turnover {0}, Average Item Price {0}", result);
        }

        [Fact]
        public void Test_GetCampaignInfo_Ended()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(6);
            var campaign = new CampaignDto
            {
                ProductId = sampleProductId,
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100,
                CreationTime = 0
            };
            _mockCampaignManager.Setup(x => x.GetCampaignInfo(It.IsAny<string>())).Returns(campaign);
            var sampleOrders = new List<OrderDto>
            {
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 3,
                    CreationTime = 0
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 4,
                    CreationTime = 1
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 5,
                    CreationTime = 5
                }
            };
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<string>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            Assert.Equal($"Campaign {campaign.Name} info; Status Ended, Target Sales {campaign.TargetSalesCount}, Total Sales {12}, Turnover {0}, Average Item Price {0}", result);
        }

        [Fact]
        public void Test_GetCampaignInfo_Zero_TotalSales()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(1);
            var campaign = new CampaignDto
            {
                ProductId = sampleProductId,
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100,
                CreationTime = 0
            };
            _mockCampaignManager.Setup(x => x.GetCampaignInfo(It.IsAny<string>())).Returns(campaign);
            var sampleOrders = new List<OrderDto>();
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<string>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            Assert.Equal($"Campaign {campaign.Name} info; Status Active, Target Sales {campaign.TargetSalesCount}, Total Sales {0}, Turnover {0}, Average Item Price -", result);
        }

        //TODO: Turnover consideration
    }
}
