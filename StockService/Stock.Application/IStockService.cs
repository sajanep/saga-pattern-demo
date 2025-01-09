using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application
{
    public interface IStockService
    {
        Task AddStock(AddStockDto addStockDto);

        Task<GetStockDto?> GetStock(int productId);
    }
}
