using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinionBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int id)
    }
}
