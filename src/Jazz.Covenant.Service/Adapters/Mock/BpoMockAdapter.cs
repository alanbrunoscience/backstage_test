using Bogus;
using Bpo;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.BpoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Bogus.DataSets;
using Jazz.Core;

namespace Jazz.Covenant.Service.Adapters.Mock
{

  public class BpoMockAdapter : IEndoserAdapter
  {
    private readonly IBpoService _bpoService;

    public BpoMockAdapter(IBpoService bpoService)
    {
      _bpoService = bpoService;
    }

    public async Task<CancelEndosermentResponseDto> CancelEndoserment(CancelEndosermentRequestDto cancelEndosermentRequestDto, string identifierInEndorser)
    {
      return new CancelEndosermentResponseDto()
      {
        GenericResponse = BpoServiceMockSuccessExcluirEmprestimo().excluiEmprestimoReturn,
        Success = true,
        Retriable = false
      };
    }

    public async Task<CancelMarginReserveDtoResponse> CancelMarginReserve(CancelMarginReserveDtoRequest cancelMarginReserveDto, string identifierInEndoser)
    {
      return new CancelEndosermentResponseDto()
      {
        GenericResponse = BpoServiceMockSuccessExcluirEmprestimo().excluiEmprestimoReturn,
        Success = true,
        Retriable = false
      };
    }


    public async Task<EndosamentMarginDtoResponse> EndosamentMargin(EndosamentMarginDtoRequest endosamentMarginDto, string identifierInEndoser)
    {
      return new EndosamentMarginDtoResponse()
      {
        GenericResponse= BpoServiceMockSuccessIncluirEmprestimo().incluiEmprestimoReturn,
        Success=true,
        Retriable=false,

      };
    }


    public async Task<MarginReserveDtoResponse> MarginReserve(MarginReserveDtoRequest marginReserveDto, string identifierInEndoser)
    {

      return new MarginReserveDtoResponse()
      {
        GenericResponse = BpoServiceMockSuccessIncluirEmprestimo().incluiEmprestimoReturn,
        Retriable = false,
        Success = true

      };

    }
    private incluiEmprestimoResponse BpoServiceMockSuccessIncluirEmprestimo()
    {

      var resultadoInclusaoEmprestimo= new Faker<ResultadoInclusaoEmprestimo>("pt_BR")
        .RuleFor(x => x.codigoEmprestimo, f=>f.Random.String(7,9,'S','d') )
        .RuleFor(x => x.dataSolicitacao,
          f => DateTime.Now)
        .RuleFor(x=>x.numeroADE,f=>f.Random.String(7,9,'S','d'))
        .Generate();
      return new incluiEmprestimoResponse(resultadoInclusaoEmprestimo) ;

    }
    private excluiEmprestimoResponse BpoServiceMockSuccessExcluirEmprestimo()
    {
      var resultadoExclusao = new ResultadoExclusaoEmprestimo();
      resultadoExclusao.situacaoExclusao = 1;
      resultadoExclusao.mensagem = "Sucesso";
      var excluiEmprestimo = new excluiEmprestimoResponse(resultadoExclusao);
      return excluiEmprestimo;
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

  }
}
