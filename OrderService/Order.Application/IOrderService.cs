using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Application.Dtos;

namespace Order.Application
{
    public interface IOrderService
    {
        Task<int> CreateOrder(CreateOrderDto createOrderDto);

        Task<GetOrderDto> GetOrder(int id);

        Task UpdateOrder(UpdateOrderDto updateOrderDto); 
    }
}
