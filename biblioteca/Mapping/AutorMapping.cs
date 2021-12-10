using biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace biblioteca.Mapping
{
    public class AutorMapping : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.Sobrenome).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.CriadoEm).HasColumnType("datetime").IsRequired();


        }
    }
}
