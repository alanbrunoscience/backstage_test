using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.TokenJwtValidated
{
    public interface ITokenValidated
    {
        bool ValidateToken(string identificador, string authToken);

    }
}
