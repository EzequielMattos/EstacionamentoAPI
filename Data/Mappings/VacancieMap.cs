using EstacionamentoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstacionamentoAPI.Data.Mappings
{
    public class VacancieMap : IEntityTypeConfiguration<Vacancie>
    {
        public void Configure(EntityTypeBuilder<Vacancie> builder)
        {
            builder.ToTable("Vacancie");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NumberVacancie).IsRequired().HasColumnName("NumberVacancie").HasColumnType("INT");
            builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.HasOne(x => x.Car).WithMany().HasForeignKey("CarId").HasConstraintName("FK_Vacancie_Car");
            builder.Property(x => x.Available).IsRequired().HasColumnName("Available").HasColumnType("INT");
            builder.Property(x => x.StartPeriod).IsRequired().HasColumnName("StartPeriod").HasColumnType("DATETIME").HasDefaultValue(DateTime.Now);
            builder.Property(x => x.EndPeriod).HasColumnName("EndPeriod").HasColumnType("DATETIME");
            builder.Property(x => x.CreateDate).IsRequired().HasColumnName("CreateDate").HasColumnType("DATETIME").HasDefaultValue(DateTime.Now);
        }
    }
}
