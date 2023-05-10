using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.DiscountCode
{
    public record ResponseDiscountCode(string Descricao, 
                                       string MensagemRetorno,
                                       string Rubrica,
                                       string RubricaComplemento,
                                       int SituacaoRetorno);
      

}
