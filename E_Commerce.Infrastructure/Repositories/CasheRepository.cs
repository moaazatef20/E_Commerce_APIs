using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class CasheRepository : ICasheRepository
    {
        private readonly IDatabase _dataBase;

        public CasheRepository(IConnectionMultiplexer connection)
        {
            _dataBase = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string cashekey, CancellationToken ct = default)
        {
            var value = await _dataBase.StringGetAsync(cashekey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string cashekey, string cashevalue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            await _dataBase.StringSetAsync(cashekey, cashevalue, timeToLive ?? TimeSpan.FromDays(7));
        }
    }
}
