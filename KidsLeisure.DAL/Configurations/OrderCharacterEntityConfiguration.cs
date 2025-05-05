using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsLeisure.DAL.Configurations
{
    public class OrderCharacterEntityConfiguration : IEntityTypeConfiguration<OrderCharacterEntity>
    {
        public void Configure(EntityTypeBuilder<OrderCharacterEntity> builder)
        {
            builder.HasKey(oc => new { oc.OrderId, oc.CharacterId });
        }
    }
}