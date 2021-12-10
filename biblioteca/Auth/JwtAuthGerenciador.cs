using biblioteca.Auth.Interfaces;
using biblioteca.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace biblioteca.Auth
{
    public class JwtAuthGerenciador : IJwtAuthGerenciador
    {
        #region Campos
        private readonly JwtConfiguracoes jwtConfiguracoes;
        #endregion

        #region Construtor
        public JwtAuthGerenciador(IOptions<JwtConfiguracoes> jwtConfiguracoes)
        {
            this.jwtConfiguracoes = jwtConfiguracoes.Value;
        }
        #endregion

        #region Métodos
        public JwtAuthModelo GerarToken(JwtCredenciais credenciais)
        {
            var declaracoes = new List<Claim>
            {
                new Claim(ClaimTypes.Email, credenciais.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, value: credenciais.Role)
            };

            var chave = Encoding.ASCII.GetBytes(jwtConfiguracoes.Segredo);

            var jwtToken = new JwtSecurityToken(
                jwtConfiguracoes.Emissor,
                jwtConfiguracoes.Audiencia,
                declaracoes,
                expires: DateTime.Now.AddMinutes(jwtConfiguracoes.ValorMinutos),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature));

            var acessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtAuthModelo
            {
                TokenAcesso = acessToken,
                TokenType = "bearer",
                ExpiraEm = jwtConfiguracoes.ValorMinutos * 60
            };
        }
        #endregion

    }
}
