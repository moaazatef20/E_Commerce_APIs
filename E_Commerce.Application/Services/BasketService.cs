using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var basketResult = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket, timeToLive, ct);
            return basketResult == null ? Result<BasketDTO>.Fail(Error.Failure("BasketCreate.Failure", "Failed to create or update basket")) : Result<BasketDTO>.OK(basket);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepository.DeleteBasketAsync(basketId, ct);
            return result ? Result<bool>.OK(true) : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Failed to delete basket"));
        }

        public async Task<Result<BasketDTO>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId, ct);
            return basket == null ? Result<BasketDTO>.Fail(Error.NotFound("BasketNotFound", "Basket not found")) : Result<BasketDTO>.OK(_mapper.Map<BasketDTO>(basket));
        }
    }
}
