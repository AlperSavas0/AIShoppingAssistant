using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAssistantAI.Models.Entities;

namespace ShoppingAssistantAI.Models.Configurations
{
    public class OrderDetailConfiguration : BaseConfiguration<OrderDetail>
    {
        public override void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            base.Configure(builder);
            builder.Ignore(x => x.ID);
            builder.HasKey(x=> new
            {
                x.OrderID,
                x.ProductID
            });

            builder.Property(x => x.UnitPrice).HasColumnType("money");
        }
    }
}
