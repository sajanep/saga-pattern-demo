using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Mappers
{
    
    public static class OrderStatusMapper
    {
        // Convert Domain enum to DTO enum
        public static Dtos.OrderStatusDto ToDto(Domain.Entities.OrderStatus domainStatus)
        {
            return (Dtos.OrderStatusDto)Enum.Parse(typeof(Dtos.OrderStatusDto), domainStatus.ToString());
        }

        // Convert DTO enum to Domain enum
        public static Domain.Entities.OrderStatus ToDomain(Dtos.OrderStatusDto dtoStatus)
        {
            return (Domain.Entities.OrderStatus)Enum.Parse(typeof(Domain.Entities.OrderStatus), dtoStatus.ToString());
        }
    }
}
