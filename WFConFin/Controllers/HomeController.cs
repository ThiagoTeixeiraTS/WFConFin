using Microsoft.AspNetCore.Mvc;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {

            var result = "retorno em texto";
            return Ok(result);

            //return BadRequest(result);
        }

        [HttpGet("info2")]
        public IActionResult GetAll2()
        {

            var result = "retorno em texto2";
            return Ok(result);

            //return BadRequest(result);
        }

        [HttpGet("info3/{valor}")]
        public IActionResult GetAll3([FromRoute] string valor)
        {
            try
            {
                return Ok(valor);

            }
            catch
            {
                return BadRequest();

            }

        }
    }
}
