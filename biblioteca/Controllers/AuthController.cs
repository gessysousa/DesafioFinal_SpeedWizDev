using biblioteca.Adaptadores;
using biblioteca.Auth.Interfaces;
using biblioteca.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace biblioteca.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Campos
        private readonly IJwtAuthGerenciador jwtAuthGerenciador;
        private readonly BibliotecaDbContext _bibliotecaDbContext;
        #endregion

        #region Construtor
        public AuthController(IJwtAuthGerenciador jwtAuthGerenciador, BibliotecaDbContext _bibliotecaDbContext)
        {
            this.jwtAuthGerenciador = jwtAuthGerenciador;
            this._bibliotecaDbContext = _bibliotecaDbContext;
        }
        #endregion

        #region Métodos
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar([FromBody] JwtUsuarioCredenciais jwtUsuarioCredenciais)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var usuarioAtual = await _bibliotecaDbContext.Usuarios.SingleOrDefaultAsync(x => x.Email == jwtUsuarioCredenciais.Email && x.Senha == jwtUsuarioCredenciais.Senha);
                
                if (usuarioAtual == null)
                {
                    return NotFound(new {
                                            Status = "Falha",
                                            Code = 400,     
                                            Data = "Usuário não encontrado"
                                        });
                }
             
                return Ok(new { data = jwtAuthGerenciador.GerarToken(usuarioAtual.ParaJwtCredenciais()) });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Mensagem = e.Message
                });
            }
        }
        #endregion
    }
}
