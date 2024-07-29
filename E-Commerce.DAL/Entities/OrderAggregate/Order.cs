using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }
        public Order(ICollection<OrderItem> orderItems, string buyerEmail, Address shipToAddress,
            DeliveryMethod deliveryMethod, decimal subtotal
            //, string paymentIntentId
            )
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            //PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Price;
        //public decimal Total { get { return Subtotal + DeliveryMethod.Price; } }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }

}
