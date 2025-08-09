using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAssistantAI.Models.Entities;

namespace ShoppingAssistantAI.Models.Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedDate).HasColumnName("Yaratılma Tarihi");
            builder.Property(x => x.ModifiedDate).HasColumnName("Guncelleme Tarihi");
            builder.Property(x => x.DeletedDate).HasColumnName("Silme Tarihi");
            builder.Property(x => x.Status).HasColumnName("Veri Durumu");
        }
    }
}
