using ap2ex2API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Domain;

namespace ap2ex2API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly IUserService _userService;

        public InvitationsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult HandleInvitation([FromBody] InvitationModel invitationModel)
        {
            if (_userService.DoesUserExist(invitationModel.To))
            {
                Contact newContact = new Contact()
                {
                    Id = invitationModel.From,
                    Name = invitationModel.From,
                    Server = invitationModel.Server
                };
                if (_userService.AddContact(invitationModel.To, newContact))
                    return CreatedAtAction(nameof(HandleInvitation), new { Id = invitationModel.From });
                else
                    return Forbid();
            }
            else
                return NotFound();
        }
    }
}