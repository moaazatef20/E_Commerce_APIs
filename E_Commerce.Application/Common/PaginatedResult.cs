using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public sealed class PaginatedResult<TEntity> 
{
        public PaginatedResult(int pageIndex, int pageSize, int count, IReadOnlyList<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; }
        public int PageSize { get;}
        public int Count { get;  }
        public IReadOnlyList<TEntity> Data { get; }
    }
}
