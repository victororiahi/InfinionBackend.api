using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Infrastructure.DTOs;

namespace InfinionBackend.Infrastructure.Services
{
    public interface IUserService
    {
        Task<ObjectResult> CreateUser(UserSignupDTO userSignupDTO);
        Task<User> Login(UserLoginDTO userLoginDTO);
        
    }
}
