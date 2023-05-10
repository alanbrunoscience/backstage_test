
namespace Jazz.Covenant.Service.PsaService.Dto.MarginReserve
{
    public record ResponseMarginReserve(
        int AnoInicioPrevisto, 
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
