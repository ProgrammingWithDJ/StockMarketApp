using APIWeb.Data;
using APIWeb.Dtos.Stocks;
using APIWeb.Interfaces;
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
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _stockRepo = stockRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocksAsync()
        {
            var stocks = await _stockRepo.GetAllStocksAsync();

            var selectedSTocks = stocks.Select(s => s.ToStockDto());
            return Ok(selectedSTocks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStockAsync([FromRoute] int Id)
        {
            var stock = await _stockRepo.GetStockByIdAsync(Id);

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
          await  _stockRepo.CreateStockAsync(stockModel);

            return CreatedAtAction(nameof(GetStockAsync), new { Id = stockModel.Id }, stockModel.ToStockDto());


        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateStockAsync([FromRoute] int Id, [FromBody] UpdateStockDto stockDto)
        {
            var stockModel = await _stockRepo.GetStockByIdAsync(Id);
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

           await  _stockRepo.UpdateStockAsync(Id, stockDto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int Id)
        {
            var stockModel = await _stockRepo.GetStockByIdAsync(Id);
            if (stockModel == null)
            {
                return NotFound();
            }
            
            await _stockRepo.DeleteStockAsync(Id);

            return NoContent();
        }
    }
}
