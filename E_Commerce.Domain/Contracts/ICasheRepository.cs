using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface ICasheRepository
    {
        Task<string?> GetAsync(string cashekey ,CancellationToken ct = default);
        Task SetAsync(string cashekey, string cashevalue,TimeSpan? timeToLive = default ,CancellationToken ct = default);
    }
}
