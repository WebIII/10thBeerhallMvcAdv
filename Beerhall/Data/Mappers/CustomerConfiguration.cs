using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beerhall.Data.Mappers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            //Table name
            builder.ToTable("Customer");

            //Primary key
            builder.HasKey(c => c.CustomerId);

            //Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            //Associations
            builder.HasOne(c => c.Location).WithMany().IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(c => c.Orders).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
