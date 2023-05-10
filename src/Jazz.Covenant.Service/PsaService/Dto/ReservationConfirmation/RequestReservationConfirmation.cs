using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.ReservationConfirmation
{
    public  record RequestReservationConfirmation(int anoInicio,
                                                  int idContratoAverbar,
                                                  int mesInicio,
                                                  string numeroDeControleInternoConsignataria,
                                                  int prazo,
                                                  string senhaColaborador,
                                                  decimal valorExtra,
                                                  decimal valorFinanciado,
                                                  decimal valorIof,
                                                  decimal valorParcela,
                                                  decimal valorRepasse
                                                  );
  

}
