using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }


        public Order(string userEmail, OrderAddrees shippingAddress, DeliveryMethod deliveryMethod, decimal subTotal, ICollection<OrderItems> items, string? paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            Items = items;
            PaymentIntentId = paymentIntentId;

        }

        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderAddrees ShippingAddress { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? PaymentIntentId { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }

        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Cost ?? 0);
    }
}
