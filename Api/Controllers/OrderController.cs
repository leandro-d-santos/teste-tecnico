using Application.Orders.Requests;
using Application.Orders.Responses;
using Application.Orders.Services;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public ActionResult<ShortOrderResponse> CreateAsync([FromBody] CreateOrderRequest request)
        {
            return Ok(orderService.CreateAsync(request));
        }

        [HttpGet]
        public ActionResult<IList<OrderResponse>> FindAsync([FromQuery] OrderSearchRequest request)
        {
            return Ok(orderService.FindAsync(request));
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> FindByIdAsync([FromRoute] int id)
        {
            OrderResponse? response = orderService.FindByIdAsync(id);
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
            return Ok(orderService.UpdateAsync(request));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAsync([FromRoute] int id)
        {
            orderService.DeleteAsync(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateStatusAsync([FromRoute] int id, [FromBody] UpdateStatusRequest request)
        {
            orderService.UpdateStatus(id, request.Status);
            return Ok();
        }
    }
}