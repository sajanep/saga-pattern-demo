using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils
{
    public static class TopicNames
    {
        public const string OrderCreated = "order.created";
        public const string OrderFailed = "order.failed";
        public const string OrderCompleted = "order.completed";
        public const string PaymentCompleted = "payment.completed";
        public const string PaymentFailed = "payment.failed";
        public const string StockReserved = "stock.reserved";
        public const string StockReservationFailed = "stock.reservationfailed";
        public const string CreateOrder = "order.create";
        public const string StockRollback = "stock.rollback";
        public const string CompletePayment = "payment.complete";
    }
}
