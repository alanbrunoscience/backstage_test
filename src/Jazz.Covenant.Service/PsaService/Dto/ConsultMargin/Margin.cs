using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.ConsultMargin
{
    public record Margin(string CodigoDeDesconto,
                         string CodigoDeDescontoComplemento,
                         string CodigoOrgao,
                         string CodigoVinculo,
                         string DescricaoModalidade,
                         string DescricaoOrgao,
                         decimal Margem,
                         string Matricula,
                         string MensagemRetorno,
                         int Modalidade,
                         string NomePessoa,
                         string SituacaoMatricula,
                         int SituacaoRetorno,
                         string UltimoFolha,
                         string Vinculo
        );
   
}
