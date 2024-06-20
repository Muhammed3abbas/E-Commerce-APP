using E_Commerce.DAL.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShipToAddress, ShipToAddress => ShipToAddress.WithOwner()); //1 : 1
            builder.Property(O => O.Status) //De 3shan ba5zan As String we byrg3 As Object Esmo Orders Status
                .HasConversion(OStatus => OStatus.ToString(),
                               OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                );
            builder.Property(O=>O.Subtotal).HasColumnType("decimal(18,2)");
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

            //builder.HasOne(O => O.DeliveryMethod).WithOne();
            //builder.HasOne(O => O.DeliveryMethod).WithMany();
            //builder.HasIndex(o => o.DeliveryMethodId).IsUnique();


        }
    }
}
