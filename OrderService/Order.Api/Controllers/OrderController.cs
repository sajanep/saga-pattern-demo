using System.Runtime.CompilerServices;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application;
using Order.Application.Dtos;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) 
        { 
            _orderService = orderService;
        }

        // POST: /api/order
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _orderService.CreateOrder(createOrderDto);
            
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderService.GetOrder(id);

            if (order == null)
                return NotFound(new { message = $"Order with ID '{id}' not found." });

            return Ok(order);
        }
    }
}
