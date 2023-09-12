using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Lista")]
        public async Task <IActionResult>Lista(){
            var listaPaises = await Task.FromResult(new List<string> 
                {"Colombia", "Argentina", "Francia", "Mexico"});
            return Ok(listaPaises);
        }
    }
}
