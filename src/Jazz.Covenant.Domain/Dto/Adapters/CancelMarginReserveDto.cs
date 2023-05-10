
namespace Jazz.Covenant.Domain.Dto.Adapters;

public class CancelMarginReserveDtoRequest
{
    public string Enrollment { get; set; }
    public string Cpf { get; set; }
    public string IdentifierNumberReserveCovenant { get; set; }
    public decimal Value { get; set; }
    public string ContractNumber { get; set; }
}

public class CancelMarginReserveDtoResponse
{
    public bool Success { get; set; }
    public bool Retriable { get; set; }
    public string ErrorMessage { get; set; }
    public object GenericResponse { get; set; }
}