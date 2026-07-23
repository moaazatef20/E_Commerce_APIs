using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Baskets
{
    public class BasketDTO
    {
        public string Id { get; set; } = default!;
        public ICollection<BasketItems> Items { get; set; } = [];
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? DeliveryCost { get; set; }
    }
}
