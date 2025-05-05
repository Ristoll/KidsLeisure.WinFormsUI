using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsLeisure.DAL.Configurations
{
    public class AttractionEntityConfiguration : IEntityTypeConfiguration<AttractionEntity>
    {
        public void Configure(EntityTypeBuilder<AttractionEntity> builder)
        {
            builder.HasKey(a => a.AttractionId);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Price).HasPrecision(18, 2);

        }
    }
}