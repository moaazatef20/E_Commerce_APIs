using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Specifications
{
    internal static class QueryCreater
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,Tkey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity,Tkey> spec) where TEntity : BaseEntity<Tkey>
        {
            var query = inputQuery;

            if(spec.Criterias != null) //3
            {
                query = query.Where(spec.Criterias);
            }

            if (spec.IncludeExpressions.Any())
            {
                query = spec.IncludeExpressions.Aggregate(query, (current, nextExp) => current.Include(nextExp));
            }

            if(spec.OrderBy !=null)
            { 
                query = query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if(spec.IsPaginated)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            return query;
        }
    }
}
