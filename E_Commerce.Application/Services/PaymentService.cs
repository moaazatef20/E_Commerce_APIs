using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings options;

        public PaymentService(IPaymentGateway paymentGateway
                              ,IBasketRepository basketRepository
                              ,IUnitOfWork unitOfWork
                              ,IOptions<PaymentGatewaySettings> options
                              ,IMapper mapper)
        {
            _paymentGateway = paymentGateway;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.options = options.Value;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct)
        {
            #region 1.Get Basket [Validate]
            var basket = await _basketRepository.GetBasketAsync(basketId, ct);
            if (basket == null)
                return Error.NotFound("Basket Not Found", $"Basket with Id {basketId} Is Not Found");
            if (basket.Items.Count == 0)
                return Error.Validation("The Basket Is Empty", "Please Add Items And Try Again");
            #endregion

            #region 2.Get Delivery Method
            if (!basket.DeliveryMethodId.HasValue)
                return Error.Validation("Delivery Method Is Required");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAysnc(new DeliveryMethodWithIdSpecification(basket.DeliveryMethodId.Value), ct);
            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method Not Found");

            basket.DeliveryCost = deliveryMethod.Cost;
            #endregion


            #region 3. Product Price
            var productIDs = basket.Items.Select(x => x.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAysnc(new ProductWithIdSpecifications(productIDs), ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found");

                product.Id = item.Id;
            }
            #endregion

            #region 4.Total Amount
            var subTotal = basket.Items.Sum(x => x.Quantity * x.Price);
            var amount = (long)((subTotal + deliveryMethod.Cost) * 100m);
            #endregion


            #region 5.Create And Update PaymentIntent
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await _paymentGateway.CreatePaymentIntentAsync(amount, options.DefaultCurrency, ct);
                basket.PaymentIntentId = result.data.PaymentIntentId;
                basket.ClientSecret = result.data.ClientSecret;
            }
            else
            {
                await _paymentGateway.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount, ct);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket); 
            #endregion

            return _mapper.Map<BasketDTO>(basket);

        }
    }
}
