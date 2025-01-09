using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.Dtos;
using SharedUtils.Messages.Base;

namespace SharedUtils.Messages
{
    public class StockRollBackMessage:BaseMessage
    {
        public StockRollBackMessage()
        {
            OrderItemList = new List<OrderItem>();
        }
        public List<OrderItem> OrderItemList { get; set; }
    }
}
