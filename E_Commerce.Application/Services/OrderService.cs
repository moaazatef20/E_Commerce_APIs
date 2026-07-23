using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string userEmail, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId, ct);
            if (basket == null) 
                return Error.NotFound("Basket not found",$"Basket with ID {orderDto.BasketId} Is not found");

            if(basket.Items.Count == 0)
                return Error.Validation("Basket is empty", $"Basket with ID {orderDto.BasketId} is empty");


            var orderItems = new List<OrderItems>(basket.Items.Count);
            var productIds = basket.Items.Select(x => x.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>()
                                           .GetAllAysnc(new ProductWithIdSpecifications(productIds))).ToDictionary(p => p.Id);

            foreach (var item in basket.Items)
            {
                if(products.TryGetValue(item.Id, out var product))
                {
                    orderItems.Add(new OrderItems
                    {
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = new ProductItemOrdered
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.PictureUrl
                        }
                    });
                }
                else
                {
                    return Error.NotFound("Product not found", $"Product with ID {item.Id} is not found");
                }
            }

            var orderAddress = _mapper.Map<OrderAddrees>(orderDto.ShipingAddress);

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                                  .GetByIdAysnc(new DeliveryMethodWithIdSpecification(orderDto.DeliveryMethodId), ct);


            if (deliveryMethod == null)
                return Error.NotFound("Delivery method not found", $"Delivery method with ID {orderDto.DeliveryMethodId} is not found");
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order(userEmail, orderAddress, deliveryMethod, subtotal, orderItems,basket.PaymentIntentId);

            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var saveResult = await _unitOfWork.SaveChangesAsync(ct);

            if (saveResult > 0)
            {
                var orderToReturn = _mapper.Map<OrderToReturnDto>(order);
                return Result<OrderToReturnDto>.OK(orderToReturn);
            }

            return Result<OrderToReturnDto>.Fail(Error.Failure("Order creation failed", "Unable to create order"));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string userEmail, CancellationToken ct = default)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAysnc(new OrderSpecification(userEmail), ct);
            if(orders.Any())
            {

                return Result<IReadOnlyList<OrderToReturnDto>>.OK(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            }
            else
            {
                Error error = Error.NotFound("Orders not found", $"No orders found for user with email {userEmail}");
                return Result<IReadOnlyList<OrderToReturnDto>>.Fail(error);
            }
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAysnc(ct);
            if (deliveryMethods != null)
            {
                return Result<IReadOnlyList<DeliveryMethodDto>>.OK(_mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
            }
            else
            {
                Error error = Error.NotFound("Delivery methods not found", "No delivery methods found");
                return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(error);
            }
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdForUserAsync(Guid orderId, string userEmail, CancellationToken ct = default)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAysnc(new OrderSpecification(orderId, userEmail), ct);
            if(order == null)
            {
                return Error.NotFound("Order not found", $"Order with ID {orderId} not found for user with email {userEmail}");
                
            }
            else
            {
                return _mapper.Map<OrderToReturnDto>(order);
                
            }
        }
    }
}
