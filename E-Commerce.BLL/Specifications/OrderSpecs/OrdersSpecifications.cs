using E_Commerce.DAL.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Specifications.OrderSpecs
{
    public class OrderSpecifications:BaseSpecification<Order>
    {
        public OrderSpecifications(string buyerEmail) :base(x=>x.BuyerEmail ==buyerEmail)
        {
            Includes.Add(x=>x.DeliveryMethod);
            Includes.Add(x=>x.OrderItems);

            AddOrderByDescending(x => x.OrderDate);
        }



        public OrderSpecifications(int id, string email)
            : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }

    }
}
