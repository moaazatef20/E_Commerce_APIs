using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface ICasheService
    {
        Task<string?> GetDataAsync(string cashekey, CancellationToken ct = default);
        Task SetDataAsync(string cashekey, object cashevalue, TimeSpan? timeToLive = null, CancellationToken ct = default);

    }
}
