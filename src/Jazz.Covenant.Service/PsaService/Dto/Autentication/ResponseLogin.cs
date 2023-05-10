using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Jazz.Covenant.Service.PsaService.Dto.Autentication;
public record ResponseLogin(string Authorization,
                                string DocFederal,
                                int IdConsignataria,
                                int IdConvenio,
                                int IdUsuario,
                                string Login,
                                string MensagemRetorno,
                                string Nome,
                                IEnumerable<string>Permissoes,
                                int SituacaoRetorno,
                                bool UsuarioValido

        );
    

