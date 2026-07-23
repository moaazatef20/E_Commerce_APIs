using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Application.Services
{
    internal class CasheService : ICasheService
    {
        private readonly ICasheRepository _casheRepository;

        public CasheService(ICasheRepository casheRepository)
        {
            _casheRepository = casheRepository;
        }

        public async Task<string?> GetDataAsync(string cashekey, CancellationToken ct = default)
        {
            return await _casheRepository.GetAsync(cashekey, ct);
        }
        public async Task SetDataAsync(string cashekey, object cashevalue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var jsonValue = JsonSerializer.Serialize(cashevalue);
            await _casheRepository.SetAsync(cashekey, jsonValue, timeToLive, ct);
        }
    }
}
