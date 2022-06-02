using ap2ex2API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ap2ex2API.Controllers
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