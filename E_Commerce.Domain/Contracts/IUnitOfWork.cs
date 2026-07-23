using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken ct = default);

        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;

    }
}
