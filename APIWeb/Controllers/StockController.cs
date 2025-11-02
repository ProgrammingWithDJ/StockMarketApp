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

        [HttpPut]
        [Route("{Id}")]
        public IActionResult UpdateStock([FromRoute] int Id, [FromBody] UpdateStockDto stockDto)
        {
            var stockModel = _context.Stock.Find(Id);
            if (stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.Industry = stockDto.Industry;
            stockModel.MarketCap = stockDto.MarketCap;
            _context.Stock.Update(stockModel);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteStock([FromRoute] int Id)
        {
            var stockModel = _context.Stock.Find(Id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(stockModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
