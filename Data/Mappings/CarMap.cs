using EstacionamentoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstacionamentoAPI.Data.Mappings
{
    public class CarMap : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Model).IsRequired().HasColumnName("Model").HasColumnType("NVARCHAR").HasMaxLength(100);
            builder.Property(x => x.Brand).IsRequired().HasColumnName("Brand").HasColumnType("VARCHAR").HasMaxLength(80);
            builder.Property(x => x.LicensePlate).IsRequired().HasColumnName("LicensePlate").HasColumnType("VARCHAR").HasMaxLength(50);
        }
    }
}
