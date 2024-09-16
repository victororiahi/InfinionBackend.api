﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data.Entities;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Interface.Service;
using Microsoft.AspNetCore.Identity;

namespace InfinionBackend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public async Task<bool> CreateUser(UserSignupDTO userSignupDTO)
        {
            //Check if the user exists
            var user = await _userManager.FindByEmailAsync(userSignupDTO.Email);
            if (user == null)
            {
                throw new Exception($"User : {userSignupDTO.Email} already exists!");
            }

            //Create the object of user entity
            var newUser = new User
            {
                FirstName = userSignupDTO.FirstName,
                LastName = userSignupDTO.LastName,
                Email = userSignupDTO.Email

            };


            //Add object from user to users table
            var result = await _userManager.CreateAsync(user, userSignupDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Something went wrong. Failed to create the user : {result.Errors.FirstOrDefault()}");
            }


            //Send Email to User


            return true;
        }

        //Login
        public async Task<LoginResponseDTO> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                //Check if user exists
                var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
                if (user == null)
                {
                    throw new Exception("Invalid username or password");
                }


                //Check if password is correct
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Invalid username or password");
                }


                await _signInManager.PasswordSignInAsync(user.Email, userLoginDTO.Password, false, false);


                //get token
                var token = _tokenService.GenerateToken(user);


                return new LoginResponseDTO
                {
                    User = user,
                    Token = token
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}


