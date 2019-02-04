using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.Ase.Tests.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid GuidId { get; set; }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.GuidId).HasColumnName("guid_id");
            builder.ToTable("test_order");
        }
    }
}
