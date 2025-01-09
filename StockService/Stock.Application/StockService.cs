using System.ComponentModel.Design;
using Stock.Domain.Entities;
using Stock.Infrastructure.Repository;

namespace Stock.Application
{
    public class StockService : IStockService
    {
        private readonly IRepository<StockEntity> _stockRepository;

        public StockService(IRepository<StockEntity> stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task AddStock(AddStockDto addStockDto)
        {
            var stock = (await _stockRepository.GetAsync(x => x.ProductId == addStockDto.ProductId)).FirstOrDefault();

            if (stock == null)
            {
                stock = new StockEntity();
                stock.ProductId = addStockDto.ProductId;
                stock.Count = addStockDto.Count;
                await _stockRepository.AddAsync(stock);
            }
            else
            {
                stock.Count += addStockDto.Count;
            }
            await _stockRepository.SaveChangesAsync();
        }

        public async Task<GetStockDto?> GetStock(int productId)
        {
            var stock = (await _stockRepository.GetAsync(x => x.ProductId == productId)).FirstOrDefault();

            if (stock != null)
            {
                var stockDto = new GetStockDto();
                stockDto.Id = stock.Id;
                stockDto.ProductId = stock.ProductId;
                stockDto.Count = stock.Count;
                return stockDto;
            }
            return null;
        }
    }
}
