
namespace Jazz.Covenant.Service.PsaService.Dto.MarginReserve;

    public record RequestMarginReserve(
        string codigoDeDesconto,
        string codigoDeDescontoComplemento,
        string matricula,
        int modalidade,
        string numeroDeControleInternoConsignataria,
        int prazo,
        decimal valorExtra,
        decimal valorFinanciado,
        decimal valorIof,
        decimal valorParcela,
        decimal valorRepasse);
  