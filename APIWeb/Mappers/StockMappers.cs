using APIWeb.Dtos.Stocks;

namespace APIWeb.Mappers
{
    public static class StockMappers
    {
        public static StocksDto ToStockDto(this Stock stockModel)
        {
            return new StocksDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }
    }
}
