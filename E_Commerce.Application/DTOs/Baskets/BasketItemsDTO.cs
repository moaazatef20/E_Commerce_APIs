using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Commerce.Application.DTOs.Baskets
{
    public class BasketItemsDTO
    {
        [Required(ErrorMessage = "Product Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Range(1, 50, ErrorMessage = "Quantity must be a positive integer")]
        public int Quantity { get; set; }
    }
}
