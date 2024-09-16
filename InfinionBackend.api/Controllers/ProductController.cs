using System.ComponentModel.DataAnnotations;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Interface.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static InfinionBackend.Infrastructure.Utitlities.Enum;

namespace InfinionBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("Invalid Parameter");
                }

                var res = _productRepository.GetProduct(id);



                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] string query = null,
            [FromQuery]Filter filter = Filter.None,
            [FromQuery]int pageNum = 1,
            [FromQuery]int pageSize = 20)
        {
            try
            {
                var res = await _productRepository.GetAll(filter,query,pageNum,pageSize);
                
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
          
        }

   
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("all fields are required");
                }

                var res = await _productRepository.AddProduct(model);
                
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateBook([Required]int id, [FromBody] ProductDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("all fields are required");
                }

                var res = await _productRepository.UpdateProduct(id,model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] [Required] int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("Invalid Parameter");
                }

                var result = await _productRepository.DeleteProduct(id);
                if (result == true)
                {
                    return Ok("Product successfully deleted!");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }


        }
    }
}
