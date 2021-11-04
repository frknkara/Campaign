using Model.Campaign;
using Moq;
using Service;
using Service.Contracts;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace ServiceTests
{
    public class CommandHandlerTests
    {
        private Mock<IProductService> _mockProductService;
        private Mock<ITimeService> _mockTimeService;
        private Mock<IOrderService> _mockOrderService;
        private Mock<ICampaignService> _mockCampaignService;

        public CommandHandlerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockCampaignService = new Mock<ICampaignService>();
            _mockOrderService = new Mock<IOrderService>();
        }

        [Fact]
        public void Test_RunCommand_Null_Command()
        {
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception exception = Assert.Throws<Exception>(() => handler.RunCommand(null));
            Assert.Equal("Command is invalid.", exception.Message);
        }

        [Fact]
        public void Test_RunCommand_Empty_String_Command()
        {
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception exception = Assert.Throws<Exception>(() => handler.RunCommand(""));
            Assert.Equal("Command is invalid.", exception.Message);
        }

        [Fact]
        public void Test_RunCommand_Command_Not_Found()
        {
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception exception = Assert.Throws<Exception>(() => handler.RunCommand("not_exist_cmd"));
            Assert.Equal("Command not found.", exception.Message);
        }

        [Fact]
        public void Test_RunCommand_Command_Not_Matched_Arguments_Count()
        {
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception exception = Assert.Throws<Exception>(() => handler.RunCommand("increase_time 1 2"));
            Assert.Equal("Number of arguments doesn't match to parameters of the command.", exception.Message);
        }

        [Fact]
        public void Test_RunCommand_Command_Not_Matched_Argument_Type()
        {
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception exception = Assert.Throws<Exception>(() => handler.RunCommand("increase_time X"));
            Assert.Equal("An argument type doesn't match to the parameter type of the command.", exception.Message);
        }

        [Fact]
        public void Test_RunCommand_Command_Should_Trim()
        {
            _mockProductService.Setup(x => x.CreateProduct(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand(" create_product phone  1 2 ");
            _mockProductService.Verify(x => x.CreateProduct("phone", 1, 2));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_Command_Uppercase()
        {
            _mockProductService.Setup(x => x.CreateProduct(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("CREATE_PRODUCT phone 1 2");
            _mockProductService.Verify(x => x.CreateProduct("phone", 1, 2));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_IncreaseTime()
        {
            var methodDelegate = new Func<int, string>(_mockTimeService.Object.IncreaseTime);
            _mockTimeService.Setup(x => x.IncreaseTime(1)).Returns("Time is 01:00");
            _mockTimeService.Setup(x => x.IncreaseTime(2)).Returns("Time is 02:00");
            _mockTimeService.Setup(x => x.IncreaseTime(24)).Returns("Time is 00:00");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("increase_time 1");
            _mockTimeService.Verify(x => x.IncreaseTime(1));
            Assert.Equal("Time is 01:00", result);
            result = handler.RunCommand("increase_time 2");
            _mockTimeService.Verify(x => x.IncreaseTime(2));
            Assert.Equal("Time is 02:00", result);
            result = handler.RunCommand("increase_time 24");
            _mockTimeService.Verify(x => x.IncreaseTime(24));
            Assert.Equal("Time is 00:00", result);
        }

        [Fact]
        public void Test_RunCommand_CreateProduct()
        {
            _mockProductService.Setup(x => x.CreateProduct(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("create_product phone 1 2");
            _mockProductService.Verify(x => x.CreateProduct("phone", 1, 2));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_GetProductInfo()
        {
            _mockProductService.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("get_product_info phone");
            _mockProductService.Verify(x => x.GetProductInfo("phone"));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_CreateOrder()
        {
            _mockOrderService.Setup(x => x.CreateOrder(It.IsAny<string>(), It.IsAny<int>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("create_order phone 3");
            _mockOrderService.Verify(x => x.CreateOrder("phone", 3));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_CreateCampaign()
        {
            _mockCampaignService.Setup(x => x.CreateCampaign(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("create_campaign black_friday phone 3 10 50");
            _mockCampaignService.Verify(x => x.CreateCampaign("black_friday", "phone", 3, 10, 50));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_GetCampaignInfo()
        {
            _mockCampaignService.Setup(x => x.GetCampaignInfo(It.IsAny<string>())).Returns("output");
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            var result = handler.RunCommand("get_campaign_info black_friday");
            _mockCampaignService.Verify(x => x.GetCampaignInfo("black_friday"));
            Assert.Equal("output", result);
        }

        [Fact]
        public void Test_RunCommand_Exception_Thrown()
        {
            Exception exception = new Exception("sample_exception");
            _mockCampaignService.Setup(x => x.GetCampaignInfo(It.IsAny<string>())).Throws(exception);
            var handler = new CommandHandler(_mockProductService.Object, _mockTimeService.Object, _mockOrderService.Object, _mockCampaignService.Object);
            Exception result = Assert.Throws<Exception>(() => handler.RunCommand("get_campaign_info black_friday"));
            _mockCampaignService.Verify(x => x.GetCampaignInfo("black_friday"));
            Assert.Equal(exception.Message, result.Message);
        }
    }
}
