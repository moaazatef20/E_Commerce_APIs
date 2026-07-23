using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct);
        Task<Result<PaymentIntentResult>> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct);
    }
}
