using APIWeb.Dtos.Stocks;

namespace APIWeb.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();

        Task<Stock?> GetStockByIdAsync(int id);

        Task <Stock> CreateStockAsync(Stock stock);

        Task DeleteStockAsync(int id);

        Task<Stock?> UpdateStockAsync(int id,UpdateStockDto UpdateStockDto);
    }
}
