using System.ComponentModel.DataAnnotations;
using InfinionBackend.Data;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinionBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([Required][FromBody] UserSignupDTO userSignupDTO)
        {
            try
            {
                var result = await _userService.CreateUser(userSignupDTO);
                if (result) return Ok("User created successfully!");

                throw new Exception("User creation failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Oooops! Something went wrong!");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([Required][FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //Check if user exists with the provided Email and Password
                var res = await _userService.Login(userLoginDTO);


                
                //Return token in the response
                return Ok(res);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Oooops! Something went wrong!");
            }
        }
    }
}

