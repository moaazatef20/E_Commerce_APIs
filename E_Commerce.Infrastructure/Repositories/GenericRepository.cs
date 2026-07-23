using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity, Tkey>(StoreDbContext dbContext) : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAysnc(CancellationToken ct = default)
            => await dbContext.Set<TEntity>().ToListAsync(ct);

        public async Task<TEntity?> GetByIdAysnc(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default)
        {
            return await QueryCreater.CreateQuery(dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync(); //4

        }

        public async Task<IReadOnlyList<TEntity>> GetAllAysnc(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default)
        {
            var query = QueryCreater.CreateQuery(dbContext.Set<TEntity>(),spec);

            return await query.ToListAsync();
        }

        public Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default)
        {
            return QueryCreater.CreateQuery(dbContext.Set<TEntity>(),spec).CountAsync(ct);
        }
    }
}
