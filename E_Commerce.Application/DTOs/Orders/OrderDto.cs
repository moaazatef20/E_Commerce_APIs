using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; } = default!;
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDTO ShipingAddress { get; set; } = default!;

    }
}
