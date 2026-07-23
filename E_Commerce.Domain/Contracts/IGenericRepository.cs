using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<TEntity ,Tkey> where TEntity :BaseEntity<Tkey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity?> GetByIdAysnc(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default);
        Task<IReadOnlyList<TEntity>> GetAllAysnc(CancellationToken ct = default);
        Task<IReadOnlyList<TEntity>> GetAllAysnc(ISpecifications<TEntity,Tkey> spec, CancellationToken ct = default);
        Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default);
    }
}
