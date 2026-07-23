using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecifications<TEntity ,Tkey> where TEntity : BaseEntity<Tkey>
    {
        ICollection<Expression<Func<TEntity,object>>> IncludeExpressions { get; }

        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }
        Expression<Func<TEntity,bool>> Criterias { get; } //1

        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }
    }
}
