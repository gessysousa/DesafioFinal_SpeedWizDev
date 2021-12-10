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
    public class AutoresController : ControllerBase
    {
        #region Campos
        private readonly BibliotecaDbContext _bibliotecaDbContext;
        #endregion

        #region Construtor
        public AutoresController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }
        #endregion

        #region Métodos
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarAutores()
        {
            var autoresList = await _bibliotecaDbContext.Autores.ToListAsync();
            return Ok(new { data = autoresList.Select(x =>
                                         new
                                         {
                                             autorId = x.Id,
                                             nome = x.Nome,
                                             sobrenome = x.Sobrenome
                                         }) });
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CadastrarAutor(AutorInput dadosEntrada)
        {
            var autor = new Autor()
            {
                Nome = dadosEntrada.Nome,
                Sobrenome = dadosEntrada.Sobrenome,
                CriadoEm = DateTime.Now
            };

            await _bibliotecaDbContext.Autores.AddAsync(autor);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(
                        new
                        {
                            Status = "Sucesso",
                            Code = 201,
                            Data = new
                            {
                                NomeAutor = autor.Nome,
                                SobrenomeAutor = autor.Sobrenome
                            }
                        }
                     );
        }
        #endregion


    }
}
