using biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace biblioteca.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar(255)").IsRequired();
            builder.Property(x => x.Senha).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.CriadoEm).HasColumnType("datetime").IsRequired();
            builder.HasOne(x => x.Role)
                    .WithMany()
                    .HasForeignKey(x => x.RoleId);
            builder.ToTable("usuarios");
        }
    }
}
