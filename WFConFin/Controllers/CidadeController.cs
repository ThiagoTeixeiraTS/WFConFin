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


    public class CidadeController : Controller
    {
        private readonly WFConfinDbContext _dbContext;

        public CidadeController(WFConfinDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCidades()
        {
            try
            {
                var result = _dbContext.Cidade.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao listar Cidades {e.Message} ");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostCidades([FromBody] Cidade cidade)
        {
            try
            {
               await _dbContext.Cidade.AddAsync(cidade);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, cidade incluida");
                }
                else
                {
                    return BadRequest("Erro, cidade não incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de Cidades {e.Message} ");
            }
        }
        [HttpPut]
        public async Task<IActionResult> PutCidades([FromBody] Cidade cidade)
        {
            try
            {
                _dbContext.Cidade.Update(cidade);
                var valor =await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, cidade incluida");
                }
                else
                {
                    return BadRequest("Erro, cidade não incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de Cidades {e.Message} ");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCidades([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _dbContext.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    _dbContext.Remove(cidade);
                    var valor = await _dbContext.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, cidade excluida");
                    }
                    else
                    {
                        return BadRequest("Erro, cidade não excluida");
                    }
                }
                else
                {
                    return NotFound("Erro, cidade não excluida pois não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão de Cidades {e.Message} ");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCidades([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _dbContext.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    return Ok(cidade);
                }
                else
                {
                    return NotFound("Erro, cidade não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de Cidades {e.Message} ");
            }
        }


        [HttpGet("Pesquisa")]
        //[Route("api/PesquisarPorEstado")]
        public async Task<IActionResult> GetCidadesPesquisa([FromQuery] string valor)
        {
            try
                {
                //query criteria
                var lista = from o in _dbContext.Cidade.ToList()
                            where o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);   
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de Cidades {e.Message} ");
            }
        }



        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetCidadePaginacao([FromQuery] string valor, int skip, int take, bool order)
        {
            try
            {
                var lista = from o in _dbContext.Cidade.ToList()
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
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

                var paginacaoResponse = new PaginacaoResponse<Cidade>(lista, qtd, skip, take);

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa Exceção: {e.Message}");
            }

        }
    }
}
