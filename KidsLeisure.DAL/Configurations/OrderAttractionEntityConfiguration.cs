using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsLeisure.DAL.Configurations
{
    public class OrderAttractionEntityConfiguration : IEntityTypeConfiguration<OrderAttractionEntity>
    {
        public void Configure(EntityTypeBuilder<OrderAttractionEntity> builder)
        {
            builder.HasKey(oa => new { oa.OrderId, oa.AttractionId });
        }
    }
}