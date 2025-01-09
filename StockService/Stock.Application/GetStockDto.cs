using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application
{
    public class GetStockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

    }
}
