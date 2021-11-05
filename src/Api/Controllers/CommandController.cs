using Microsoft.AspNetCore.Mvc;
using Service;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly CommandHandler _commandHandler;
        private readonly ResetSystemDataHandler _resetSystemDataHandler;

        public CommandController(CommandHandler commandHandler, ResetSystemDataHandler resetSystemDataHandler)
        {
            _commandHandler = commandHandler;
            _resetSystemDataHandler = resetSystemDataHandler;
        }

        [HttpPost()]
        public ActionResult<string> Execute([FromForm]string command)
        {
            try
            {
                var result = _commandHandler.RunCommand(command);
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpPost()]
        [Route("reset")]
        public ActionResult<string> ResetSystemData()
        {
            try
            {
                _resetSystemDataHandler.ResetDb();
                return "Resetting is successful.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
