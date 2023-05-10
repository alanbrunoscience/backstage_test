using Jazz.Covenant.Service.PsaService.Dto.Autentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Autentication
{
    public interface IPsaAutentication
    {
        Task<string> GetToken(int idCovenant);
    }
}
