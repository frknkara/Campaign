using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Service
{
    public class CommandHandler
    {
        private readonly Dictionary<string, Delegate> _commands = new Dictionary<string, Delegate>();

        public CommandHandler(IProductService productService, 
            ITimeService timeService, 
            IOrderService orderService, 
            ICampaignService campaignService)
        {
            _commands = new Dictionary<string, Delegate>();
            _commands["create_product"] = new Func<string, int, int, string>(productService.CreateProduct);
            _commands["get_product_info"] = new Func<string, string>(productService.GetProductInfo);
            _commands["create_order"] = new Func<string, int, string>(orderService.CreateOrder);
            _commands["create_campaign"] = new Func<string, string, int, int, int, string>(campaignService.CreateCampaign);
            _commands["get_campaign_info"] = new Func<string, string>(campaignService.GetCampaignInfo);
            _commands["increase_time"] = new Func<int, string>(timeService.IncreaseTime);
        }

        public string RunCommand(string commandStr)
        {
            if (string.IsNullOrWhiteSpace(commandStr))
                throw new Exception("Command is invalid.");
            commandStr = commandStr.Trim();
            var commandStrArray = commandStr.Split();
            var commandKey = commandStrArray[0];
            commandKey = commandKey.ToLowerInvariant();
            var commandArgsStr = commandStrArray.Where(x => !string.IsNullOrWhiteSpace(x)).Skip(1).ToArray<object>();
            if (!_commands.ContainsKey(commandKey))
                throw new Exception("Command not found.");
            var command = _commands[commandKey];
            var argTypes = command.Method.GetParameters().Select(x => x.ParameterType).ToList();

            var args = new object[argTypes.Count];
            if (argTypes.Count != commandArgsStr.Length)
                throw new Exception("Number of arguments doesn't match to parameters of the command.");

            for (var i = 0; i < argTypes.Count; i++)
            {
                try
                {
                    args[i] = Convert.ChangeType(commandArgsStr[i], argTypes[i]);
                }
                catch
                {
                    throw new Exception("An argument type doesn't match to the parameter type of the command.");
                }
            }
            try
            {
                return command.DynamicInvoke(args).ToString();
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
