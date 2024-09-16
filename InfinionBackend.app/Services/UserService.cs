using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data.Entities;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Interface.Service;

namespace InfinionBackend.Infrastructure.Services
{
    public class UserService : IUserService
        {
            private readonly SignInManager<User> _signInManager;
            private readonly RoleManager<Role> _roleManager;
            private readonly UserManager<User> _userManager;
            public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _signInManager = signInManager;
            }


            public async Task<ObjectResult> CreateUser(UserSignupDTO userSignupDTO)
            {
                //Check if role to be assigned the user exists
                var roleFound = await _roleManager.FindByNameAsync(userSignupDTO.UserRole);
                if (roleFound == null)
                {
                    return new BadRequestObjectResult($"The {userSignupDTO.UserRole} you have entered does not exist");
                }

                //Create the object of user entity
                var user = new User
                {
                    FirstName = userSignupDTO.FirstName,
                    LastName = userSignupDTO.LastName,
                    PhoneNumber = userSignupDTO.PhoneNumber,
                    Email = userSignupDTO.Email,
                    NormalizedEmail = userSignupDTO.Email.ToUpper(),
                    NormalizedUserName = userSignupDTO.Email.ToUpper(),
                    UserName = userSignupDTO.Email,
                    EmailConfirmed = true
                };

                //Add object from user to users table
                var result = await _userManager.CreateAsync(user, userSignupDTO.Password);
                if (!result.Succeeded)
                {
                    return new UnprocessableEntityObjectResult("Something went wrong. Failed to create the user");
                }

                //Assign the user a role
                var userRoleResult = await _userManager.AddToRoleAsync(user, userSignupDTO.UserRole);
                if (!userRoleResult.Succeeded)
                {
                    return new UnprocessableEntityObjectResult("Could not assign the user a role");
                }

                return new OkObjectResult("User has been created successfully");


            }

            //Login
            public async Task<User> Login(UserLoginDTO userLoginDTO)
            {
                try
                {
                    //Check if user exists
                    var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
                    if (user == null)
                    {
                        return null;
                    }

                    //Check if password is correct
                    var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);
                    if (result == PasswordVerificationResult.Failed)
                    {
                        return null;
                    }

                    await _signInManager.PasswordSignInAsync(user.Email, userLoginDTO.Password, false, false);
                    return user;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            //Get Roles
            public async Task<List<string>> GetRolesForUser(User user)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }

            public async Task<List<User>> GetUsers()
            {
                try
                {
                    var users = await _userManager.Users.ToListAsync();
                    return users;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

}
}
