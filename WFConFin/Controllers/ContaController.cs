using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public class ContaController : Controller
    {
        private readonly WFConfinDbContext _dbContext;

        public ContaController(WFConfinDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContas()
        {
            try
            {
                var result = _dbContext.Conta.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao listar Cidades {e.Message} ");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostConta([FromBody] Conta conta)
        {
            try
            {
                await _dbContext.Conta.AddAsync(conta);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, Conta incluida");
                }
                else
                {
                    return BadRequest("Erro, Conta não incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de Cidades {e.Message} ");
            }
        }
        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutCidades([FromBody] Conta conta)
        {
            try
            {
                _dbContext.Conta.Update(conta);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, conta incluida");
                }
                else
                {
                    return BadRequest("Erro, conta não incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de conta {e.Message} ");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteCidades([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _dbContext.Conta.FindAsync(id);
                if (conta != null)
                {
                    _dbContext.Remove(conta);
                    var valor = await _dbContext.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, conta excluida");
                    }
                    else
                    {
                        return BadRequest("Erro, conta não excluida");
                    }
                }
                else
                {
                    return NotFound("Erro, conta não excluida pois não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão de conta {e.Message} ");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContas([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _dbContext.Conta.FindAsync(id);
                if (conta != null)
                {
                    return Ok(conta);
                }
                else
                {
                    return NotFound("Erro, conta não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de conta {e.Message} ");
            }
        }


        [HttpGet("Pessoa/{PessoaId}")]
        public async Task<IActionResult> GetContasPessoa([FromRoute] Guid PessoaId)
        {
            try
            {
                //query criteria
                var lista = from o in _dbContext.Conta.Include(o => o.Pessoa).ToList()
                            where o.PessoaId == PessoaId
                            select o;

          
                if (lista != null)
                {
                    return Ok(lista);
                }
                else
                {
                    return NotFound("Erro, conta não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de conta {e.Message} ");
            }
        }


        [HttpGet("Pesquisa")]
        //[Route("api/PesquisarPorEstado")]
        public async Task<IActionResult> GetContasPesquisa([FromQuery] string valor)
        {
            try
            {
                //query criteria
                var lista = from o in _dbContext.Conta.ToList()
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de Cidades {e.Message} ");
            }
        }



        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool order)
        {
            try
            {
                var lista = from o in _dbContext.Conta.ToList()
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            //|| o.DataVencimento.ToUpper().Contains(valor.ToUpper())
                            select o;


                if (order)
                {
                    lista = from o in lista
                            orderby o.Descricao ascending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Descricao descending
                            select o;
                }
                var qtd = lista.Count();

                lista = lista.Skip(skip).Take(take).ToList();

                var paginacaoResponse = new PaginacaoResponse<Conta>(lista, qtd, skip, take);

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa Exceção: {e.Message}");
            }

        }


    }

}
