using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dtos
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public OrderStatusDto Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
