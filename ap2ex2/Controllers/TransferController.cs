using ap2ex2.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ap2ex2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IUserService _userService;

        public TransferController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult HandleTransfer([FromBody] TransferModel transferModel)
        {
            if (_userService.ReceiveMessage(transferModel.Content, transferModel.From, transferModel.To))
                return CreatedAtAction(nameof(HandleTransfer), new { Content = transferModel.Content });
            else
                return Forbid();
        }
    }
}