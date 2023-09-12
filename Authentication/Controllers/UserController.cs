using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Authentication.Models.Custom;
using Authentication.Services;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthorizationService _authorizationService;

        public UserController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("Autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] AuthorizationRequest autorizacion){
            var resultado_autorizacion = await _authorizationService.DevolverToken(autorizacion);
            if(resultado_autorizacion == null){
                return Unauthorized();
            }
            return Ok(resultado_autorizacion);
        }
    }
}
