using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IBasketService
    {
        Task<Result<BasketDTO>> GetBasketAsync(string basketId, CancellationToken ct = default);
        Task<Result<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basket, TimeSpan? timeToLive = default, CancellationToken ct = default);
        Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default);
    }
}
