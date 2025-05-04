using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsLeisure.DAL.Configurations
{
    public class ZoneEntityConfiguration : IEntityTypeConfiguration<ZoneEntity>
    {
        public void Configure(EntityTypeBuilder<ZoneEntity> builder)
        {
            builder.HasKey(z => z.ZoneId);
            builder.Property(z => z.Name).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Price).HasPrecision(18, 2);

        }
    }
}