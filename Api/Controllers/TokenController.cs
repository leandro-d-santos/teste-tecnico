using Application.Tokens.Requests;
using Application.Tokens.Responses;
using Application.Tokens.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class TokensController(ITokenSettingsService tokenSettingsService) : ControllerBase
    {
        private readonly ITokenSettingsService _tokenSettingsService = tokenSettingsService;

        [HttpGet]
        public ActionResult<IList<TokenSearchResponse>> FindAll()
        {
            return Ok(_tokenSettingsService.FindAll());
        }

        [HttpPost]
        public ActionResult<TokenResponse> Create([FromBody] TokenSettingsRequest request)
        {
            return Ok(_tokenSettingsService.Create(request));
        }

        [HttpDelete("{id}")]
        public ActionResult Revoke([FromRoute] int id)
        {
            _tokenSettingsService.Revoke(id);
            return Ok();
        }
    }
}
