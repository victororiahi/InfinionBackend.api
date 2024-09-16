using System.ComponentModel.DataAnnotations;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinionBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
            private readonly IUserService _userService;
            private readonly ITokenService _tokenService;
            public UserController(IUserService userService, ITokenService tokenService)
            {
                _userService = userService;
                _tokenService = tokenService;
            }

        public IUserService Get_userService()
        {
            return _userService;
        }


        [HttpPost("Register")]
            public async Task<IActionResult> Register([Required][FromBody] UserSignupDTO userSignupDTO, IUserService _userService)
            {
                try
                {
                    var result = await _userService.CreateUser(userSignupDTO);
                    return result;
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
                    var user = await _userService.Login(userLoginDTO);


                    //If user credentials are not authentic, return Bad Request with a message
                    if (user == null)
                    {
                        return this.Problem("Username or password is incorrect", statusCode: 400);
                    }


                    //Generate Jwt token
                    var token = _tokenService.GenerateToken(user, roles);

                    //Return token in the response
                    return Ok(new { Token = token });
                }

                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Oooops! Something went wrong!");
                }
            }
        }
    }

}
