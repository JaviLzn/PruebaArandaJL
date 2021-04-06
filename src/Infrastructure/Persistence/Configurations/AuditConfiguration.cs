using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.HasComment("Tabla para almacenar historio al dar clic en obtener datos");


            builder.Property(p => p.Info)
                  .IsRequired();

            builder.Property(p => p.CreatedBy)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.CreatedOn)
                   .HasMaxLength(40)
                   .IsRequired();
        }
    }
}
