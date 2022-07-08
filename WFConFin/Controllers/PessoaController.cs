using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PessoaController : Controller
    {
        private readonly WFConfinDbContext _dbContext;

        public PessoaController(WFConfinDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPessoa()
        {
            try
            {
                var result = _dbContext.Pessoa.ToList();
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro - {e.Message}");
            }
        }



        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                await _dbContext.Pessoa.AddAsync(pessoa);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Pessoa foi incluida!");

                }
                else
                {
                    return BadRequest("Erro ao Add Pessoa!");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro - {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                _dbContext.Pessoa.Update(pessoa);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Pessoa foi Alterada!");

                }
                else
                {
                    return BadRequest("Erro ao Alterar Pessoa!");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro - {e.Message}");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeletePessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _dbContext.Pessoa.FindAsync(id);
                if (pessoa != null)
                {
                    _dbContext.Pessoa.Remove(pessoa);
                    var valor = await _dbContext.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Pessoa foi Excluida!");

                    }
                    else
                    {
                        return BadRequest("Erro ao excluir pessoa");
                    }
                }
                else
                {
                    return NotFound("Erro Pessoa não foi encontrada!");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro - {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _dbContext.Pessoa.FindAsync(id);
                if (pessoa != null)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return NotFound("Erro, pessoa não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de Cidades {e.Message} ");
            }
        }


        [HttpGet("Pesquisa")]
        //[Route("api/PesquisarPorEstado")]
        public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor)
        {
            try
            {
                //query criteria
                var lista = from o in _dbContext.Pessoa.ToList()
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.Email.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta as pessoas {e.Message} ");
            }
        }



        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string valor, int skip, int take, bool order)
        {
            try
            {
                var lista = from o in _dbContext.Pessoa.ToList()
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                                || o.Email.ToUpper().Contains(valor.ToUpper())
                            select o;


                if (order)
                {
                    lista = from o in lista
                            orderby o.Nome ascending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Nome descending
                            select o;
                }
                var qtd = lista.Count();

                lista = lista.Skip(skip).Take(take).ToList();

                var paginacaoResponse = new PaginacaoResponse<Pessoa>(lista, qtd, skip, take);

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa Exceção: {e.Message}");
            }

        }

    }
}
