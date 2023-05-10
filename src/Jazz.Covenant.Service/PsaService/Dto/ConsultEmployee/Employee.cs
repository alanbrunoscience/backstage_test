using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.ConsultEmployee
{
    public record Employee(string CodigoOrgao,
                           string CodigoVinculo,
                           string Cpf,
                           string DataNascimento,
                           string DescricaoOrgao,
                           string DescricaoVinculo,
                           string Matricula,
                           string MensagemRetorno,
                           string Nome,
                           string NomeMae,
                           string NomePai,
                           int SituacaoRetorno,
                           string UltimaFolha,
                           string CodigoCargo,
                           string DescricaoCargo,
                           string DodigoLotacao,
                           string DescricaoLotacao,
                           long DataAmissao
                           );
    
}
