using BillingRadar.Application.Modules.Auth.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingRadar.WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginQuery request)
        {
            var result = await _sender.Send(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
