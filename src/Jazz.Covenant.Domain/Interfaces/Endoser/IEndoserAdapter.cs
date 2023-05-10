using Jazz.Core;
using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Domain.Interfaces.Endoser
{
    public interface IEndoserAdapter
    {
        Task<IEnumerable<EmployeeDto>?> ListInformationEnrollment(object idCovenantEndoser, string cpf);
        Task<ApiResult<MarginLoanDto?>> ConsultMarginLoan(object idCovenantEndoser, string cpf,string enrollment,string covenantAutorization);

        Task<ApiResult<MarginCreditCardDto>> ConsultMarginCreditCard(string idCovenant, string enrollment,
            string cpf, string autorization);
        Task<MarginReserveDtoResponse> MarginReserve(MarginReserveDtoRequest marginReserveDto, string identifierInEndoser);
        Task<EndosamentMarginDtoResponse> EndosamentMargin(EndosamentMarginDtoRequest endosamentMarginDto, string identifierInEndoser);
        Task<CancelMarginReserveDtoResponse> CancelMarginReserve(CancelMarginReserveDtoRequest cancelMarginReserveDto, string identifierInEndoser);
        Task<CancelEndosermentResponseDto> CancelEndoserment(CancelEndosermentRequestDto cancelEndosermentRequestDto, string identifierInEndorser);
    }
}
