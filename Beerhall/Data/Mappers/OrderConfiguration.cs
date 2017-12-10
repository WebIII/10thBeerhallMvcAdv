using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beerhall.Data.Mappers
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Table name
            builder.ToTable("Order");

            //Primary key
            builder.HasKey(o => o.OrderId);

            //Properties
            builder.Property(o => o.Street)
                .IsRequired()
                .HasMaxLength(100);

            //Associations
            builder.HasMany(t => t.OrderLines).WithOne().HasForeignKey(t => t.OrderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Location).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
