using APIWeb.Data;
using APIWeb.Dtos.Stocks;
using APIWeb.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAllStocksAsync()
        {
            var stocks = await _context.Stock.ToListAsync();

            var selectedSTocks = stocks.Select(s => s.ToStockDto());
            return Ok(selectedSTocks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStockAsync([FromRoute] int Id)
        {
            var stock = await _context.Stock.FindAsync(Id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockAsync([FromBody] CreateStockDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stock.Add(stockModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStockAsync), new { Id = stockModel.Id }, stockModel.ToStockDto());


        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateStockAsync([FromRoute] int Id, [FromBody] UpdateStockDto stockDto)
        {
            var stockModel = await _context.Stock.FindAsync(Id);
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
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int Id)
        {
            var stockModel = await _context.Stock.FindAsync(Id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
