using E_Commerce.Domain.Entities.Orders;

namespace E_Commerce.Application.Specifications
{
    internal class DeliveryMethodWithIdSpecification : BaseSpecification<DeliveryMethod, int>
    {
        public DeliveryMethodWithIdSpecification(int id) : base(dm => dm.Id == id)
        {
        }
    }
}
