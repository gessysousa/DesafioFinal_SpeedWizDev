using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.Context
{
    public class BibliotecaDbContext : DbContext
    {
        #region Propriedades

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Livro> Livros { get; set; }
        #endregion

        #region Construtor
        public BibliotecaDbContext(DbContextOptions options) : base(options)
        {

        }
        #endregion

        #region Métodos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }


}
