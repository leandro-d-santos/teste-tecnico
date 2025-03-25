using Application.Orders.Requests;
using Application.Orders.Responses;
using Application.Orders.Services;
using Domain.Tokens.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [ServiceFilter(typeof(TokenAuth))]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost]
        public ActionResult<ShortOrderResponse> CreateAsync([FromBody] CreateOrderRequest request)
        {
            return Ok(_orderService.CreateAsync(request));
        }

        [HttpGet]
        public ActionResult<IList<OrderResponse>> FindAsync([FromQuery] OrderSearchRequest request)
        {
            return Ok(_orderService.FindAsync(request));
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> FindByIdAsync([FromRoute] int id)
        {
            OrderResponse? response = _orderService.FindByIdAsync(id);
            if (response is null)
            {
                return BadRequest("Recurso não encontrado");
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<ShortOrderResponse> UpdateAsync([FromRoute] int id, [FromBody] UpdateOrderRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(String.Format("Parâmetro inválido: {0}", nameof(id)));
            }
            return Ok(_orderService.UpdateAsync(request));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAsync([FromRoute] int id)
        {
            _orderService.DeleteAsync(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateStatusAsync([FromRoute] int id, [FromBody] UpdateStatusRequest request)
        {
            _orderService.UpdateStatus(id, request.Status);
            return Ok();
        }
    }
}