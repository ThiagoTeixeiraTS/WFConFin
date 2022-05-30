using Microsoft.AspNetCore.Mvc;

namespace WFConFin.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class HomeController : Controller
    {

        public IActionResult GetAll()
        {

            var result = "retorno em texto";
            return Ok(result);

            //return BadRequest(result);
        }


    }
}
