using E_Commerce.DAL.Entities.OrderAggregate;

namespace E_Commerce.Api.Dtos
{
    public class OrderToReturnDto
    {

        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        //public decimal DeliveryMethodCost { get; set; }

        public decimal ShippingPrice { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }



        //public int? DeliveryMethodId { get; set; }

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
