﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinionBackend.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}
