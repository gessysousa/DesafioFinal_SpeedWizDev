using biblioteca.Services;

namespace biblioteca.Auth.Interfaces
{
    public interface IJwtAuthGerenciador
    {
        JwtAuthModelo GerarToken(JwtCredenciais credenciais);
    }
}
