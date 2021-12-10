using biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace biblioteca.Mapping
{
    public class LivroMapping : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Descricao).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.ISBN).IsRequired();
            builder.Property(x => x.AnoLancamento).IsRequired();
            builder.Property(x => x.CriadoEm).HasColumnType("datetime").IsRequired();
            builder.HasOne(x => x.Autor)
                    .WithMany()
                    .HasForeignKey(x => x.AutorId);
            builder.ToTable("livros");
        }
    }
}
