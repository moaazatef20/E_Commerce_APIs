using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId,CancellationToken ct);
    }
}
