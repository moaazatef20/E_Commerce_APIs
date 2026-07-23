using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.Property(oi => oi.Price)
                   .HasColumnType("decimal(8,2)");

            builder.OwnsOne(x => x.Product, pn =>
            {
                pn.Property(x => x.ProductName)
                  .HasMaxLength(100);

                pn.Property(x => x.PictureUrl)
                  .HasMaxLength(200);
            });

        }
    }
}