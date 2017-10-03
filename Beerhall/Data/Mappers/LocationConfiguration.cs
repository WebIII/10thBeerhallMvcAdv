using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beerhall.Data.Mappers
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            //Table name
            builder.ToTable("Location");

            //Primary Key
            builder.HasKey(l => l.PostalCode);

            //Properties
            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
