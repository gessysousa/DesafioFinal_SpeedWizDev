using biblioteca.Context;
using biblioteca.Input;
using biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuariosController : ControllerBase
    {
        #region Campos
        private readonly BibliotecaDbContext _bibliotecaDbContext;
        #endregion

        #region Construtor
        public UsuariosController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }
        #endregion

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CadastrarUsuario(UsuarioInput dadosEntrada)
        {
            var role = await _bibliotecaDbContext.Roles.Where(x => x.Id == dadosEntrada.RoleId).FirstOrDefaultAsync();
            if (role == null)
            {
                return NotFound(
                                    new
                                    {
                                        Status = "Falha",
                                        Code = 400,
                                        Data = "Role não encontrado"
                                    }
                                );
            }
            var usuario = new Usuario()
            {
                RoleId = dadosEntrada.RoleId,
                Nome = dadosEntrada.Nome,
                Email = dadosEntrada.Email,
                Senha = dadosEntrada.Senha,
                CriadoEm = DateTime.Now
            };

            await _bibliotecaDbContext.Usuarios.AddAsync(usuario);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(
                        new
                        {
                            Status = "Sucesso",
                            Code = 201,
                            Data = true
                        }
                );
        }
    }
}
