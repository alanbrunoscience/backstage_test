using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.Autentication;

public record Login(
                    int idConvenio,
                    string usuario,
                    string senha
                    );


