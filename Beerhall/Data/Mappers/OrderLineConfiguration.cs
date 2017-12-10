using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beerhall.Data.Mappers
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            //Table name
            builder.ToTable("OrderLine");

            //Primary key
            builder.HasKey(t => new {
                t.OrderId,
                t.ProductId
            });

            //Associations
            builder.HasOne(ol => ol.Product).WithMany().IsRequired().HasForeignKey(o => o.ProductId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
