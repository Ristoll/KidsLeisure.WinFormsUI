using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsLeisure.DAL.Configurations
{
    public class OrderZoneEntityConfiguration : IEntityTypeConfiguration<OrderZoneEntity>
    {
        public void Configure(EntityTypeBuilder<OrderZoneEntity> builder)
        {
            builder.HasKey(oz => new { oz.OrderId, oz.ZoneId });
        }
    }
}