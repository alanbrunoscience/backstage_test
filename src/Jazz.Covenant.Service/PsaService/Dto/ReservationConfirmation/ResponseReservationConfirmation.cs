using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.ReservationConfirmation
{
    public record ResponseReservationConfirmation(int AnoInicioPrevisto,
                                                  string DataAverbacao,
                                                  string DataInclusao,
                                                  int IdContrato,
                                                  string Matricula,
                                                  string MensagemRetorno,
                                                  int MesInicioPrevisto,
                                                  string NumeroDeControleInternoConsignataria,
                                                  int Prazo,
                                                  int SituacaoRetorno,
                                                  string ValidadeReserva,
                                                  decimal ValorParcela);
  

}
