namespace Order.Application.Dtos
{
    public class CreateOrderDto
    {
        public CreateOrderDto()
        {
            OrderItemList = new List<OrderItemDto>();
        }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public List<OrderItemDto> OrderItemList { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
