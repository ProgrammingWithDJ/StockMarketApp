using APIWeb.Data;
using APIWeb.Dtos.Stocks;
using APIWeb.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace APIWeb.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStocks()
        {
            var stocks = _context.Stock.ToList()
                .Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{Id}")]
        public IActionResult GetStock([FromRoute] int Id)
        {
            var stock = _context.Stock.Find(Id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult CreateStock([FromBody] CreateStockDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStock), new { Id = stockModel.Id }, stockModel.ToStockDto());


        }
    }
}
