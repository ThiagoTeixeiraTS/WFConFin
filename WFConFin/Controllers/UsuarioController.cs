using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WFConFin.Data;
using WFConFin.Models;
using WFConFin.Services;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class UsuarioController : Controller
    {
        private readonly WFConfinDbContext _dbContext;

        private readonly TokenService _tokenService;

        public UsuarioController(WFConfinDbContext dbContext, TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            var usuario = _dbContext.Usuario.Where(x => x.Login == usuarioLogin.Login).FirstOrDefault();
            
            if(usuario == null)
                return NotFound("Usuario Inválido!");

            var passwordHash = MD5Hash.CalcHash(usuarioLogin.Password);


            if (usuario.Password != passwordHash)
                return BadRequest("Senha Invalida!");

           var token = _tokenService.GerarToken(usuario);

            usuario.Password = "";

            var result = new UsuarioResponse()
            {
                Usuario = usuario,
                Token = token
            };

            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var result = _dbContext.Usuario.ToList();
                return Ok(result);
            }
            catch (System.Exception e )
            {

                return BadRequest(e.Message);
            }
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarios([FromRoute] Guid id)
        {
            try
            {
                Usuario usuario = await _dbContext.Usuario.FindAsync(id);
                if (usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("Erro, usuario não foi localizado");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Consulta de usuario {e.Message} ");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            try
            {
                
                string passwordHash = MD5Hash.CalcHash(usuario.Password);
                usuario.Password = passwordHash;    

                await _dbContext.Usuario.AddAsync(usuario);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, usuario incluida");
                }
                else
                {
                    return BadRequest("Erro, usuario não incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de Cidades {e.Message} ");
            }
        }
        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
        {
            try
            {
                string passwordHash = MD5Hash.CalcHash(usuario.Password);
                usuario.Password = passwordHash;

                _dbContext.Usuario.Update(usuario);
                var valor = await _dbContext.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, usuario incluida");
                }
                else
                {
                    return BadRequest("Erro, conta usuario incluida");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de usuario {e.Message} ");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] Guid id)
        {
            try
            {
                Usuario usuario = await _dbContext.Usuario.FindAsync(id);
                if (usuario != null)
                {
                    _dbContext.Remove(usuario);
                    var valor = await _dbContext.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, usuario excluida");
                    }
                    else
                    {
                        return BadRequest("Erro, usuario não excluida");
                    }
                }
                else
                {
                    return NotFound("Erro, usuario não excluida pois não foi localizada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão de usuario {e.Message} ");
            }
        }

    }
}
