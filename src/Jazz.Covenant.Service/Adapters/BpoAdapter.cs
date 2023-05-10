using System.Net;
using Bpo;
using Jazz.Core;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.BpoService;
using Jazz.Covenant.Service.Mappers;

namespace Jazz.Covenant.Service.Adapters
{
    public class BpoAdapter : IEndoserAdapter
    {
        private readonly IBpoService _bpoService;

        public BpoAdapter(IBpoService bpoService)
        {
            _bpoService = bpoService;
        }

        public async Task<ApiResult<MarginLoanDto>> ConsultMarginLoan(object idCovenantEndoser, string cpf, string enrollment,
            string covenantAutorization)
        {

            var dataClient =
                await _bpoService.ConsultMarginConsignableByCPFEnrollmentAutorization(idCovenantEndoser.ToString(),
                    enrollment, cpf, covenantAutorization);

            if(!dataClient.Success)
                return new ApiResult<MarginLoanDto?>(new MarginLoanDto(Convert.ToDecimal(0)), dataClient.ApiName,
                    dataClient.Status, dataClient.Errors);
            if (dataClient.Result.consultaMargemConsignavelReturn.Count(c => c.matricula == enrollment) == 0)
            {
                return new ApiResult<MarginLoanDto?>(new MarginLoanDto(Convert.ToDecimal(0)), dataClient.ApiName,
                    HttpStatusCode.BadRequest, new List<DomainMessage> {
                        new DomainMessage("Enrollment not exist","Enrollment not exist on this TaxId")});
            }


            var margin = dataClient.Result.consultaMargemConsignavelReturn.Select(cM =>
              {

                     var margin = cM.margens.Where(m => m.tipoProduto == 1).FirstOrDefault();
                    return margin.valorDisponivel;
                }
            ).FirstOrDefault();
            return new ApiResult<MarginLoanDto>(new MarginLoanDto(Convert.ToDecimal(margin)), dataClient.ApiName,
                dataClient.Status, dataClient.Errors);
        }

        public async Task<IEnumerable<EmployeeDto>> ListInformationEnrollment(object idCovenantEndoser, string cpf)
        {
            var consulta = await _bpoService.ConsultMarginConsignableByCpf(idCovenantEndoser.ToString(), cpf);

            return consulta.consultaMargemConsignavelReturn.Select(c =>
                new EmployeeDto(c.cpf, c.matricula, c.secretaria));
        }

        public async Task<ApiResult<MarginCreditCardDto>> ConsultMarginCreditCard(string idCovenant, string enrollment,
            string cpf, string autorization)
        {
            var marginCreditCard = await _bpoService.
                ConsultMarginCreditCard(idCovenant, enrollment, cpf, autorization);

            if(!marginCreditCard.Success)
                return new ApiResult<MarginCreditCardDto>(new MarginCreditCardDto(0), marginCreditCard.ApiName,
                    marginCreditCard.Status, marginCreditCard.Errors);

            if (marginCreditCard.Result.consultaMargemCartaoReturn.matricula != enrollment)
                return new ApiResult<MarginCreditCardDto>(new MarginCreditCardDto(0), marginCreditCard.ApiName,
                    HttpStatusCode.BadRequest, new List<DomainMessage> {
                        new DomainMessage("Enrollment not exist","Enrollment not exist on this TaxId")});

            var marginCreditCardDto = new MarginCreditCardDto(marginCreditCard.Result.consultaMargemCartaoReturn.valorMargem);

            return new ApiResult<MarginCreditCardDto>(
                new MarginCreditCardDto(marginCreditCardDto.Margin), marginCreditCard.ApiName,
                marginCreditCard.Status);
        }

        public async Task<MarginReserveDtoResponse> MarginReserve(MarginReserveDtoRequest marginReserveDto, string identifierInEndoser)
        {
            var incluiEmprestimo = DomainDtoToBPOMappers.ToBPOIncluiEmprestimoRequest(marginReserveDto);

            var responseBPO = await _bpoService.MarginReserve(incluiEmprestimo, identifierInEndoser);

            if (responseBPO is null)
            {
                return new MarginReserveDtoResponse
                {
                    Success = false,
                    Retriable = true,
                    ErrorMessage = "BPO response null"
                };
            }

            var responseDto = new MarginReserveDtoResponse
            {
                Success = string.IsNullOrEmpty(responseBPO.incluiEmprestimoReturn.mensagemErro),
                Retriable = false,
                ErrorMessage = responseBPO.incluiEmprestimoReturn.mensagemErro,
                GenericResponse = responseBPO
            };

            return responseDto;
        }

        public async Task<EndosamentMarginDtoResponse?> EndosamentMargin(EndosamentMarginDtoRequest endosamentMarginDto,
            string identifierInEndoser)
        {
            var addLoan =
                DomainDtoToBPOMappers.EndorsamentMarginDtoRequestToBPOIncluiEmprestimoRequest(endosamentMarginDto);
            var endosamentReserveResponse = await _bpoService.EndosamentMargin(addLoan, identifierInEndoser);
            if (endosamentReserveResponse.incluiEmprestimoReturn.mensagemErro != string.Empty)
                return new EndosamentMarginDtoResponse()
                {
                    ErrorMessage = "Problem with margin endosament",
                    Success = false,
                    Retriable = true,
                    GenericResponse = endosamentReserveResponse.incluiEmprestimoReturn
                };
            return new EndosamentMarginDtoResponse()
            {
                Success = true,
                GenericResponse = endosamentReserveResponse,
                Retriable = false
            };
        }

        public async Task<CancelMarginReserveDtoResponse> CancelMarginReserve(CancelMarginReserveDtoRequest cancelMarginReserveDto, string identifierInEndoser)
        {
            var excluiEmprestimoRequest = new excluiEmprestimoRequest();
            excluiEmprestimoRequest.matricula = cancelMarginReserveDto.Enrollment;
            excluiEmprestimoRequest.cpf = cancelMarginReserveDto.Cpf;
            excluiEmprestimoRequest.numeroADE = cancelMarginReserveDto.IdentifierNumberReserveCovenant;
            excluiEmprestimoRequest.valor = decimal.ToDouble(cancelMarginReserveDto.Value);
            excluiEmprestimoRequest.numeroContrato = cancelMarginReserveDto.ContractNumber;

            var responseBPO = await _bpoService.CancelMarginReserve(excluiEmprestimoRequest, identifierInEndoser);

            if (responseBPO is null)
            {
                return new CancelMarginReserveDtoResponse
                {
                    Success = false,
                    Retriable = true,
                    ErrorMessage = "BPO response null"
                };
            }

            var responseDto = new CancelMarginReserveDtoResponse
            {
                Success = responseBPO.excluiEmprestimoReturn.codigoErro == 0,
                Retriable = false,
                ErrorMessage = responseBPO.excluiEmprestimoReturn.mensagem,
                GenericResponse = responseBPO
            };

            return responseDto;
        }

        public async Task<CancelEndosermentResponseDto> CancelEndoserment(CancelEndosermentRequestDto cancelEndosermentRequestDto,string identifierInEndorser)
        {
            var excluiEmprestimoRequest = new excluiEmprestimoRequest();
            excluiEmprestimoRequest.matricula = cancelEndosermentRequestDto.Enrollment;
            excluiEmprestimoRequest.cpf = cancelEndosermentRequestDto.Cpf;
            excluiEmprestimoRequest.numeroADE = cancelEndosermentRequestDto.IdentifierNumberReserveCovenant;
            excluiEmprestimoRequest.valor = decimal.ToDouble(cancelEndosermentRequestDto.Value);
            excluiEmprestimoRequest.numeroContrato = cancelEndosermentRequestDto.ContractNumber;
            var responseBPO = await _bpoService.CancelMarginReserve(excluiEmprestimoRequest, identifierInEndorser);

            if (responseBPO.excluiEmprestimoReturn.codigoErro !=0)
            {
                return new CancelEndosermentResponseDto
                {
                    Success = false,
                    Retriable = true,
                    ErrorMessage = "BPO response null"
                };
            }
            var responseDto = new CancelEndosermentResponseDto
            {
                Success = responseBPO.excluiEmprestimoReturn.codigoErro == 0,
                Retriable = false,
                ErrorMessage = responseBPO.excluiEmprestimoReturn.mensagem,
                GenericResponse = responseBPO
            };
            return responseDto;
        }
    }
}
