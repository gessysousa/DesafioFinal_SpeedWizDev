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
    public class LivrosController : ControllerBase
    {
        #region Campos
        private readonly BibliotecaDbContext _bibliotecaDbContext;
        #endregion

        #region Construtor
        public LivrosController(BibliotecaDbContext bibliotecaDbContext)
        {
            _bibliotecaDbContext = bibliotecaDbContext;
        }
        #endregion

        #region Métodos
       

        [HttpGet("listar")]
        [Authorize]
        public async Task<IActionResult> ListarLivros()
        {
            var livros = await _bibliotecaDbContext.Livros
                                .Include(x => x.Autor)
                                .ToListAsync();
            return Ok(new
            { 
                data = livros.Select(x =>
                                new
                                {
                                    Id = x.Id, //inserido para conseguir fazer o atualiza
                                    autor = x.AutorId, //inserido para conseguir fazer o atualiza
                                    descricao = x.Descricao,
                                    ISBN = x.ISBN,
                                    AnoLancamento = x.AnoLancamento,
                                    NomeAutor = x.Autor.Nome + " " + x.Autor.Sobrenome
                                })
           });
        }

        [HttpPost("cadastrar")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CadastrarLivro(LivroInput dadosEntrada)
        {
            var autor = await _bibliotecaDbContext.Autores.Where(x => x.Id == dadosEntrada.AutorId).FirstOrDefaultAsync();
            if (autor == null)
            {
                return NotFound(
                                    new
                                    {
                                        Status = "Falha",
                                        Code = 400,
                                        Data = "Autor não encontrado"
                                    }
                                );
            }
            var livro = new Livro()
            {
                AutorId = dadosEntrada.AutorId,
                Descricao = dadosEntrada.Descricao,
                ISBN = dadosEntrada.ISBN,
                AnoLancamento = dadosEntrada.AnoLancamento,
                CriadoEm = DateTime.Now
            };

            await _bibliotecaDbContext.Livros.AddAsync(livro);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(
                        new
                        {
                            Status = "Sucesso",
                            Code = 201,
                            Data = new
                                     {
                                        NomeLivro = livro.Descricao,
                                        ISBN = livro.ISBN,
                                        AnoLancamento = dadosEntrada.AnoLancamento
                            }
                        }
                );
        }

        [HttpPut("atualizar")]
        [Authorize]
        public async Task<IActionResult> Atualizar(AtualizarLivroInput dadosEntrada)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.Id == dadosEntrada.Id).FirstOrDefaultAsync();
            if (livro == null)
            {
                return NotFound(
                                    new
                                    {
                                        Status = "Falha",
                                        Code = 400,
                                        Data = "Não encontrado"
                                    }
                                );
            }
            livro.Descricao = dadosEntrada.Descricao;
            livro.AnoLancamento = dadosEntrada.AnoLancamento;

            _bibliotecaDbContext.Livros.Update(livro);
            await _bibliotecaDbContext.SaveChangesAsync();

            return Ok(new
            {
                Status = "Sucesso",
                Code = 200,
                Data = new
                {
                    NomeLivro = livro.Descricao,
                    AnoLancamento = livro.AnoLancamento
                }
            });
        }


        [HttpDelete("deletar/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Deletar(int id)
        {
            var livro = await _bibliotecaDbContext.Livros.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (livro == null)
            {
                return NotFound(new
                {
                    Status = "Falha",
                    Code = 400,
                    Data = "Não encontrado"
                });
            }
            _bibliotecaDbContext.Livros.Remove(livro);
            await _bibliotecaDbContext.SaveChangesAsync();
            return Ok(new
            {
                Status = "Sucesso",
                Code = 201,
                Data = true
            });
        }
        #endregion


    }
}
