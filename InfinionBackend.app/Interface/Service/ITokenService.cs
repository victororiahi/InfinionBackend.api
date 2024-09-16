using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
us

namespace InfinionBackend.Infrastructure.Interface.Service
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}
