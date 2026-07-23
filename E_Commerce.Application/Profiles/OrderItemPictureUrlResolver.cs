using AutoMapper;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Profiles
{
    internal class OrderItemPictureUrlResolver : IValueResolver<OrderItems, OrderItemDto, string>
    {
        private readonly UrlSettings _settings;

        public OrderItemPictureUrlResolver(IOptions<UrlSettings> options)
        {
            _settings = options.Value;
        }
        public string Resolve(OrderItems source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd("/");
            var path = source.Product.PictureUrl.TrimStart("/");
            return $"{baseUrl}/Files/{path}";
        }
    }
}
