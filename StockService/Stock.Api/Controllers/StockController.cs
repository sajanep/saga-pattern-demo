using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Application;

namespace Stock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        // POST: /api/order
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddStockDto addStockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _stockService.AddStock(addStockDto);

            return Ok();
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            var stockDto = await _stockService.GetStock(productId);

            if (stockDto == null)
            {
                return NotFound(new { message = $"Stock for product id '{productId}' not found." });
            }
            return Ok(stockDto);
        }
    }
}
