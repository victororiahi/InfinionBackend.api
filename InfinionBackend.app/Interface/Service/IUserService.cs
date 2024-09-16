using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data.Entities;
using InfinionBackend.Infrastructure.DTOs;

namespace InfinionBackend.Infrastructure.Interface.Service
{
    public interface IUserService
    {   
        Task<bool> CreateUser(UserSignupDTO userSignupDTO);
        Task<LoginResponseDTO> Login(UserLoginDTO userLoginDTO);

    }
}
