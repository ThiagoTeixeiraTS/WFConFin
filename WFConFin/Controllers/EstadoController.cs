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
    public class EstadoController : Controller
    {
        private readonly WFConfinDbContext _dbContext;
        public EstadoController(WFConfinDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                var result =  _dbContext.Estado.ToList();
                return  Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest("Erro na Listagem dos estados Exceção: " + e.Message);
            }

        }



        [HttpPost]
        public async Task<IActionResult> PostEstado([FromBody] Estado estado)
        {
            try
            {
                await _dbContext.Estado.AddAsync(estado);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso !");
                }
                else
                {
                    return BadRequest($"Erro, estado não Incluido!");
                }

            }
            catch (Exception e)
            {

                return BadRequest($"Erro Estado não incluido Exceção: {e.Message}");
            }

        }

        [HttpPut]
        public async Task<IActionResult>PutEstado([FromBody] Estado estado)
        {
            try
            {
                _dbContext.Estado.Update(estado);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso !");
                }
                else
                {
                    return BadRequest($"Erro, estado não alterado!");
                }

            }
            catch (Exception e)
            {

                return BadRequest($"Erro Estado não foi alterado Exceção: {e.Message}");
            }

        }

        [HttpDelete("{sigla}")]
        public async Task<IActionResult> DeleteEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = await _dbContext.Estado.FindAsync(sigla);
                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    _dbContext.Estado.Remove(estado);
                    var valor = await _dbContext.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, Estado excluido !");
                    }
                    else
                    {
                        return BadRequest($"Erro, estado não excluido!");
                    }
                }
                else
                {
                    return NotFound("Erro, Estado não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro Estado não foi alterado Exceção: {e.Message}");
            }

        }

        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = await _dbContext.Estado.FindAsync(sigla);
                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    return Ok(estado);
                }
                else
                {
                    return NotFound("Erro, Estado não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na consulta do Estado Exceção: {e.Message}");
            }

        }


        [HttpGet("Pesquisa")]
        public async Task< IActionResult> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from o in _dbContext.Estado.ToList()
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;
                return Ok(lista);


            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa Exceção: {e.Message}");
            }

        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string valor, int skip, int take, bool order)
        {
            try
            {
                var lista = from o in _dbContext.Estado.ToList()
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
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

                var paginacaoResponse = new PaginacaoResponse<Estado>(lista, qtd, skip, take);

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa Exceção: {e.Message}");
            }

        }
    }
}
