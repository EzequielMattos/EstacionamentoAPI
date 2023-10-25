using EstacionamentoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstacionamentoAPI.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Cars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("NVARCHAR").HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasColumnName("Email").HasColumnType("VARCHAR").HasMaxLength(80);
            builder.Property(x => x.CNH).IsRequired().HasColumnName("CNH").HasColumnType("VARCHAR").HasMaxLength(25);
            builder.Property(x => x.CPF).IsRequired().HasColumnName("CPF").HasColumnType("VARCHAR").HasMaxLength(25);
            builder.Property(x => x.Phone).IsRequired().HasColumnName("Phone").HasColumnType("VARCHAR").HasMaxLength(25);
            builder.Property(x => x.CreateDate).IsRequired().HasColumnName("CreateDate").HasColumnType("DATETIME").HasDefaultValue(DateTime.Now);
            builder.HasMany(x => x.Cars).WithMany(x => x.Customers).UsingEntity<Dictionary<string, object>>
                (
                "CustomerCar",
                car => car.HasOne<Car>().WithMany().HasForeignKey("CarId").HasConstraintName("FK_CustomerCar_CarId").OnDelete(DeleteBehavior.Cascade),
                customer => customer.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").HasConstraintName("FK_CustomerCar_CustomerId").OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
