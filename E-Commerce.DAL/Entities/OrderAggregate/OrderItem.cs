using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {

        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, double price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }





    }
}
