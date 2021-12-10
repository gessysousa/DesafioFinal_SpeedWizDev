using biblioteca.Models;
using biblioteca.Services;
using System;

namespace biblioteca.Adaptadores
{
    public static class ModeloAdaptadores
    {
        public static JwtCredenciais ParaJwtCredenciais(this Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException();
            }
            return new JwtCredenciais
            {
                Email = usuario.Email,
                Senha = usuario.Senha,
                Role = usuario.RoleId.ToString()
            };
        }
    }
}
