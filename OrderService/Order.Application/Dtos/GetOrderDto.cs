
namespace Order.Application.Dtos
{
    public class GetOrderDto
    {
        public GetOrderDto()
        {
            OrderItemList = new List<OrderItemDto>();
        }
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public OrderStatusDto Status { get; set; }
        public string? ErrorMessage { get; set; }
        public virtual List<OrderItemDto> OrderItemList { get; set; }
    }

    public enum OrderStatusDto
    {
        Pending = 0,
        Complete = 1,
        Fail = 2
    }
}
