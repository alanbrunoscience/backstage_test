using Jazz.Core;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.PsaService;

namespace Jazz.Covenant.Service.Adapters
{
    public class PsaAdpater : IEndoserAdapter
    {
        private readonly IPsaService _psaService;
        public PsaAdpater(IPsaService psaService)
        {
            _psaService = psaService;
        }


        public Task<ApiResult<MarginLoanDto?>> ConsultMarginLoan(object idCovenantEndoser, string cpf, string enrollment, string covenantAutorization)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<MarginCreditCardDto>> ConsultMarginCreditCard(string idCovenant, string enrollment, string cpf, string autorization)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<EmployeeDto>> ListInformationEnrollment(object idCovenantEndoser, string cpf)
        {
            var informsEnrollment = await _psaService.ConsultEmployeeByCpf((int)idCovenantEndoser, cpf);
            return informsEnrollment.Select(inf =>
            new EmployeeDto(cpf, inf.Matricula, inf.DescricaoOrgao)
            );
        }

        public Task<MarginReserveDtoResponse> MarginReserve(MarginReserveDtoRequest marginReserveDto, string identifierInEndoser)
        {
            throw new NotImplementedException();
        }

        public Task<EndosamentMarginDtoResponse> EndosamentMargin(EndosamentMarginDtoRequest endosamentMarginDto, string identifierInEndoser)
        {
            throw new NotImplementedException();
        }

        public Task<CancelMarginReserveDtoResponse> CancelMarginReserve(CancelMarginReserveDtoRequest cancelMarginReserveDto, string identifierInEndoser)
        {
            throw new NotImplementedException();
        }

        public Task<CancelEndosermentResponseDto> CancelEndoserment(CancelEndosermentRequestDto cancelEndosermentRequestDto, string identifierInEndorser)
        {
            throw new NotImplementedException();
        }
    }
}
