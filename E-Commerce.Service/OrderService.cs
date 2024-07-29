using E_Commerce.BLL.Interfaces;
using E_Commerce.DAL.Entities.OrderAggregate;
using E_Commerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.BLL.Specifications.OrderSpecs;
namespace E_Commerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {

            /* Steps To Create Order
             1- Get Basket From Basket Reop
             2- Get Selected Items at Basked From Products Repo
             3- calculate SubTotal
             4- Get Delivery Method from Delivery Method  Repo
             5- Create Order 
             6- Save To Data Base
             
             
             */
            
            // get basket from repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get items from the product repo
            var items = new List<OrderItem>();
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                    var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                    items.Add(orderItem);
                }
            }

            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // check to see if order exists
            //var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            //var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            //var order = await _unitOfWork.Repository<Order>().;

            //if (order != null)
            //{
            //    order.ShipToAddress = shippingAddress;
            //    order.DeliveryMethod = deliveryMethod;
            //    order.Subtotal = subtotal;
            //    _unitOfWork.Repository<Order>().Update(order);
            //}
            //else
            //{
                // create order
                var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod,
                    (Decimal)subtotal);
                //order = new Order(items, buyerEmail, shippingAddress, deliveryMethod,
                //        (Decimal)subtotal, basket.PaymentIntentId);
                _unitOfWork.Repository<Order>().Add(order);
            //}

            // save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GelAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderSpecifications(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);

            return await _unitOfWork.Repository<Order>().GelAllWithSpecAsync(spec);
        }
    }
}
