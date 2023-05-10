using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.PsaService.Dto.Contract
{
    public record Contract(int AnoFim,
                           int AnoInicio,
                           string AverbadoPor,
                           string CodigoDeDesconto,
                           string CodigoDeDescontoComplemento,
                           string Cpf,
                           string DataAverbacao,
                           string DataReserva,
                           int ExisteIntencao,
                           int IdContrato,
                           string Matricula,
                           string MensagemRetorno,
                           int MesFim,
                           int MesInicio,
                           int Modalidade,
                           string ModalidadeDescricao,
                           string Nome,
                           string NomeConsignataria,
                           string NumeroDeControleInternoConsignataria,
                           int Prazo,
                           int QuantidadeParcelaDesconta,
                           string ReservadorPor,
                           int SituacaoRetorno,
                           string StatusContrato,
                           string TipoContrato,
                           decimal ValorAverbado,
                           decimal ValorParcelaAverbada,
                           decimal ValorParcelaReservada,
                           decimal ValorReservado
                           );
    
}
