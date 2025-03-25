using Application.Clients.Requests;
using Application.Clients.Responses;
using Application.Clients.Services;
using Domain.Tokens.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/clients")]
    [ServiceFilter(typeof(TokenAuth))]
    public class ClientsController(IClientService clientService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;

        [HttpPost]
        public ActionResult<ClientResponse> CreateAsync([FromBody] CreateClientRequest request)
        {
            return Ok(_clientService.CreateAsync(request));
        }

        [HttpGet]
        public ActionResult<IList<ClientSearchResponse>> FindAsync([FromQuery] ClientSearchRequest request)
        {
            return Ok(_clientService.FindAsync(request));
        }

        [HttpGet("{id}")]
        public ActionResult<ClientByIdResponse> FindByIdAsync([FromRoute] int id)
        {
            ClientByIdResponse? response = _clientService.FindByIdAsync(id);
            if (response is null)
            {
                return BadRequest("Recurso não encontrado");
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<ClientResponse> UpdateAsync([FromRoute] int id, [FromBody] UpdateClientRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(String.Format("Parâmetro inválido: {0}", nameof(id)));
            }
            return Ok(_clientService.UpdateAsync(request));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAsync([FromRoute] int id)
        {
            _clientService.DeleteAsync(id);
            return Ok();
        }
    }
}