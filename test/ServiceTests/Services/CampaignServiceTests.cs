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
                CreationTime = 0,
                InitialPrice = 200
            };
            _mockProductManager.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns(sampleProduct);
        }

        [Fact]
        public void Test_CreateCampaign_Active_Campaign_Exist()
        {
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(5);
            var activeCampaign = new CampaignDto
            {
                ProductId = sampleProductId,
                Duration = 5,
                Name = "campaign",
                PriceManipulationLimit = 20,
                TargetSalesCount = 100
            };
            _mockProductManager.Setup(x => x.GetActiveCampaign(It.IsAny<ProductDto>(), It.IsAny<int>())).Returns(activeCampaign);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            Exception exception = Assert.Throws<Exception>(() => service.CreateCampaign("campaign", sampleProduct.Code, 5, 20, 100));
            _mockProductManager.Verify(x => x.GetProductInfo(sampleProduct.Code));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            _mockProductManager.Verify(x => x.GetActiveCampaign(sampleProduct, 5));
            Assert.Equal($"There is an active campaign with {activeCampaign.Name} name. New campaign can't be created for this product.", exception.Message);
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
            CampaignDto activeCampaign = null;
            _mockProductManager.Setup(x => x.GetActiveCampaign(It.IsAny<ProductDto>(), It.IsAny<int>())).Returns(activeCampaign);
            _mockCampaignManager.Setup(x => x.CreateCampaign(It.IsAny<CreateCampaignDto>())).Returns(campaign);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.CreateCampaign(createCampaign.Name, createCampaign.ProductCode, createCampaign.Duration, createCampaign.PriceManipulationLimit, createCampaign.TargetSalesCount);
            _mockProductManager.Verify(x => x.GetProductInfo(createCampaign.ProductCode));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            _mockProductManager.Verify(x => x.GetActiveCampaign(sampleProduct, 0));
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
                    CreationTime = 0,
                    UnitPrice = 200
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 4,
                    CreationTime = 1,
                    UnitPrice = 150
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 5,
                    CreationTime = 5,
                    UnitPrice = 100
                }
            };
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<CampaignDto>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            //Turnover 3*200 + 4*150 + 5*100, total sales 3 + 4 + 5, average 141.666666667
            Assert.Equal($"Campaign {campaign.Name} info; Status Active, Target Sales {campaign.TargetSalesCount}, Total Sales 12, Turnover 1700, Average Item Price 141.67", result);
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
                    CreationTime = 0,
                    UnitPrice = 2000
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 4,
                    CreationTime = 1,
                    UnitPrice = 2000
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 5,
                    CreationTime = 5,
                    UnitPrice = 1000
                }
            };
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<CampaignDto>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            //Turnover 3*2000 + 4*2000 + 5*1000, total sales 3 + 4 + 5, average 1583.3333333334
            Assert.Equal($"Campaign {campaign.Name} info; Status Ended, Target Sales {campaign.TargetSalesCount}, Total Sales 12, Turnover 19000, Average Item Price 1583.33", result);
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
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<CampaignDto>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            Assert.Equal($"Campaign {campaign.Name} info; Status Active, Target Sales {campaign.TargetSalesCount}, Total Sales 0, Turnover 0, Average Item Price -", result);
        }


        [Fact]
        public void Test_GetCampaignInfo_Integer_AverageItemPrice()
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
                    Quantity = 5,
                    CreationTime = 0,
                    UnitPrice = 200
                },
                new OrderDto
                {
                    ProductId = sampleProductId,
                    Quantity = 5,
                    CreationTime = 1,
                    UnitPrice = 100
                }
            };
            _mockCampaignManager.Setup(x => x.GetCampaignOrders(It.IsAny<CampaignDto>())).Returns(sampleOrders);
            var service = new CampaignService(_mockCampaignManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetCampaignInfo(campaign.Name);
            //Turnover 5*200 + 5*100, total sales 5 + 5
            Assert.Equal($"Campaign {campaign.Name} info; Status Active, Target Sales {campaign.TargetSalesCount}, Total Sales 10, Turnover 1500, Average Item Price 150", result);
        }
    }
}
