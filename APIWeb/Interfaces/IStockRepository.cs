using APIWeb.Dtos.Stocks;
using APIWeb.Helpers;

namespace APIWeb.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync(QueryObject query);

        Task<Stock?> GetStockByIdAsync(int id);

        Task <Stock> CreateStockAsync(Stock stock);

        Task DeleteStockAsync(int id);

        Task<Stock?> UpdateStockAsync(int id,UpdateStockDto UpdateStockDto);

        Task<bool> StockExists(int id);
    }
}
