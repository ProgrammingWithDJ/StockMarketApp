using APIWeb.Data;
using APIWeb.Dtos.Stocks;
using APIWeb.Helpers;
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
        public async Task<List<Stock>> GetAllStocksAsync(QueryObject queryObject)
        {
            var stocks = _context.Stock.Include(c => c.Comments).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains( queryObject.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.orderBy))
            {
                // Expecting values like: "CompanyName", "CompanyName desc", "Symbol", "MarketCap desc", etc.
                var order = queryObject.orderBy.Trim();
                bool desc = order.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                var key = desc ? order.Substring(0, order.Length - 5).Trim() : order;

                switch (key.ToLowerInvariant())
                {
                    case "companyname":
                    case "company":
                        stocks = desc ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                        break;
                    case "symbol":
                        stocks = desc ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                        break;
                    case "purchase":
                        stocks = desc ? stocks.OrderByDescending(s => s.Purchase) : stocks.OrderBy(s => s.Purchase);
                        break;
                    case "lastdiv":
                        stocks = desc ? stocks.OrderByDescending(s => s.LastDiv) : stocks.OrderBy(s => s.LastDiv);
                        break;
                    case "industry":
                        stocks = desc ? stocks.OrderByDescending(s => s.Industry) : stocks.OrderBy(s => s.Industry);
                        break;
                    case "marketcap":
                    case "market":
                        stocks = desc ? stocks.OrderByDescending(s => s.MarketCap) : stocks.OrderBy(s => s.MarketCap);
                        break;
                    default:
                        // unknown key: no ordering change (or optionally apply a safe default)
                        break;
                }
            }

                return await stocks.ToListAsync();
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
