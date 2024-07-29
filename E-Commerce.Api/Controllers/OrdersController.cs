using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Api.Errors;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Specifications.OrderSpecs;
using E_Commerce.DAL.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{


    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            //var email = "QWE@FW.COM";

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            var Order = _mapper.Map<OrderToReturnDto>(order);
            //var o = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(Order);
        }

        [HttpGet] // /api/Orders?email:asda@eeq.com
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            //var email = User.RetrieveEmailFromPrincipal();
            var email = User.FindFirstValue(ClaimTypes.Email);
            var spec = new OrderSpecifications(email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            var Orders = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            //return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            return Ok(Orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            //var email = User.RetrieveEmailFromPrincipal();
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));
            //return Ok(order);   
            var Order = _mapper.Map<OrderToReturnDto>(order);
            return Order;
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }

}

