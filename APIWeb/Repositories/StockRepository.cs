using APIWeb.Data;
using APIWeb.Dtos.Stocks;
using APIWeb.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIWeb.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<List<Stock>> GetAllStocksAsync()
        {
           return await _context.Stock.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            var stock = await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null)
            {
                return null;
            }

            return stock;
        }

        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            _context.Stock.Add(stock);
            await  _context.SaveChangesAsync();
            return stock;
        }

        public async Task DeleteStockAsync(int id)
        {
            var stock =  await _context.Stock.FindAsync(id);
            if(stock != null)
            {
                _context.Stock.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockDto stock)
        {
            var existingStock = await _context.Stock.FindAsync(id);
            if(existingStock == null)
            {
                return null;
            }
            existingStock.Symbol = stock.Symbol;
            existingStock.CompanyName = stock.CompanyName;
            existingStock.Purchase = stock.Purchase;
            existingStock.LastDiv = stock.LastDiv;
            existingStock.Industry = stock.Industry;
            existingStock.MarketCap = stock.MarketCap;
            _context.Stock.Update(existingStock);
            await _context.SaveChangesAsync();
            return existingStock;
        }

        public Task<bool> StockExists(int id)
        {
           return _context.Stock.AnyAsync(e => e.Id == id);
        }
    }
}
