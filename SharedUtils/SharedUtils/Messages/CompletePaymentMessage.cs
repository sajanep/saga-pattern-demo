﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.Dtos;
using SharedUtils.Messages.Base;

namespace SharedUtils.Messages
{
    public class CompletePaymentMessage:BaseMessage
    {
        public CompletePaymentMessage() 
        { 
            OrderItemList = new List<OrderItem>();
        }

        public Guid CorrelationId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItemList { get; set; }
    }
}
