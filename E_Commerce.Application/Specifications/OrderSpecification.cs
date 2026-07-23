using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class OrderSpecification : BaseSpecification<Order,Guid>
    {
        public OrderSpecification(string email):base(x=>x.UserEmail == email)
        {
            AddInClude(x => x.DeliveryMethod);
            AddInClude(x => x.Items);
            AddOrderByDesc(x => x.OrderDate);
        }
        public OrderSpecification(Guid id ,string email): base(x => x.Id == id && x.UserEmail == email)
        {
            AddInClude(x => x.DeliveryMethod);
            AddInClude(x => x.Items);
            AddOrderByDesc(x => x.OrderDate);
        }
    }
}
