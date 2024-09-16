using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data.Entities;

namespace InfinionBackend.Infrastructure.DTOs
{
    public class LoginResponseDTO
    {
        public  User User { get; set; }
        public  string Token { get; set; }
    }
}
