using System.Net;
using System.ServiceModel;
using Bpo;
using Jazz.Core;
using Jazz.Covenant.Domain.Constants;
using Jazz.Covenant.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Jazz.Covenant.Service.BpoService
{
  public class BpoService : IBpoService
    {
        private const string NOME_BANCO = "Banco Arbi";
        private readonly IOptions<Settings.Bpo> _bpoOption;
        private string UrlBase;
        public BpoService(IOptions<Settings.Bpo> options)
        {
          _bpoOption = options;
          UrlBase = _bpoOption.Value.BASE_URL;
        }

        public async Task<incluiEmprestimoResponse> MarginReserve(
            incluiEmprestimoRequest incluiEmprestimo,
            string identifierInEndoser)
        {
            incluiEmprestimo.convenio = identifierInEndoser;
            incluiEmprestimo.nomeBanco = BankName.BankArbi;
            incluiEmprestimo.tipoOperacao = (int) OperationType.MarginReserve;
            incluiEmprestimo.usuarioAcesso = _bpoOption.Value.User;
            incluiEmprestimo.senhaAcesso = _bpoOption.Value.Password;


            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);

            var resp = await client.incluiEmprestimoAsync(incluiEmprestimo);

            return resp;
        }

        public async Task<ApiResult<consultaMargemCartaoResponse>> ConsultMarginCreditCard(string idCovenant, string enrollment,
            string cpf, string autorization)
        {
            try
            {
                consultaMargemCartaoRequest consultaMargemCartaoRequest = new consultaMargemCartaoRequest()
                {
                    convenio = idCovenant,
                    cpf = cpf,
                    matricula = enrollment,
                    identificadorMargem = autorization,
                    nomeBanco = BankName.BankArbi,
                    usuarioAcesso = _bpoOption.Value.User,
                    senhaAcesso = _bpoOption.Value.Password
                };

                var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);

                var resp = await client.consultaMargemCartaoAsync(consultaMargemCartaoRequest);

                var apiResult = new ApiResult<consultaMargemCartaoResponse>(resp,"",HttpStatusCode.OK);
                return apiResult;
            }
            catch (FaultException<ConsignacaoException> e)
            {
                return new ApiResult<consultaMargemCartaoResponse?>(null,"",HttpStatusCode.BadRequest,
                    new List<DomainMessage> {
                        new DomainMessage(
                            e.Detail.code.ToString(),
                            HelperFromMessageExceptionBpo.ErroMessageConsultMargin(e.Message)) });
            }
        }

        public async Task<consultaMargemCartaoResponse?> ConsultMarginCardCrediByEnrollment(string idCovenant,
            string enrollment)
        {
            consultaMargemCartaoRequest consultaMargemCartaoRequest = new consultaMargemCartaoRequest()
            {
                convenio = $"{idCovenant}",
                cpf = "?",
                matricula = enrollment,
                nomeBanco = NOME_BANCO,
                usuarioAcesso = _bpoOption.Value.User,
                senhaAcesso = _bpoOption.Value.Password
            };
            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);

            return await client.consultaMargemCartaoAsync(consultaMargemCartaoRequest);
        }

        public async Task<consultaMargemCartaoResponse?> ConsultMarginCardCreditByCpf(string idCovenant, string cpf)
        {
            consultaMargemCartaoRequest consultaMargemCartaoRequest = new consultaMargemCartaoRequest()
            {
                convenio = $"{idCovenant}",
                cpf = cpf,
                matricula = "?",
                nomeBanco = NOME_BANCO,
                usuarioAcesso = _bpoOption.Value.User,
                senhaAcesso = _bpoOption.Value.Password
            };
            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
            return await client.consultaMargemCartaoAsync(consultaMargemCartaoRequest);
        }

        public async Task<consultaMargemCartaoResponse?> ConsultMarginCardCreditByCPFEnrollmentAutorization(
            string idCovenant, string enrollment, string cpf, string autorization)
        {
            consultaMargemCartaoRequest consultaMargemCartaoRequest = new consultaMargemCartaoRequest()
            {
                convenio = $"{idCovenant}",
                cpf = cpf,
                matricula = enrollment,
                nomeBanco = NOME_BANCO,
                identificadorMargem = autorization,
                usuarioAcesso = _bpoOption.Value.User,
                senhaAcesso = _bpoOption.Value.Password
            };
            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
            return await client.consultaMargemCartaoAsync(consultaMargemCartaoRequest);
        }

        public async Task<consultaMargemConsignavelResponse?> ConsultMarginConsignableByCpf(string idCovenant,
            string cpf)
        {
            consultaMargemConsignavelRequest consultaMargemConsignavelRequest = new consultaMargemConsignavelRequest()
            {
                convenio = $"{idCovenant}",
                cpf = cpf,
                matricula = "?",
                tipoOperacao = 1,
                nomeBanco = NOME_BANCO,
                usuarioAcesso = _bpoOption.Value.User,
                senhaAcesso = _bpoOption.Value.Password
            };
            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
            return await client.consultaMargemConsignavelAsync(consultaMargemConsignavelRequest);
        }

        public async Task<ApiResult<consultaMargemConsignavelResponse?>>ConsultMarginConsignableByCPFEnrollmentAutorization(string idCovenant, string enrollment,string cpf,string autorization)
        {
            try
            {
                consultaMargemConsignavelRequest consultaMargemConsignavelRequest = new consultaMargemConsignavelRequest()
                {
                    convenio = $"{idCovenant}",
                    cpf = cpf,
                    matricula = enrollment,
                    tipoOperacao = 1,
                    nomeBanco = NOME_BANCO,
                    identificadorMargem = autorization,
                    usuarioAcesso = _bpoOption.Value.User,
                    senhaAcesso = _bpoOption.Value.Password
                };
                var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
                var response=await client.consultaMargemConsignavelAsync(consultaMargemConsignavelRequest);
                var apiResult = new ApiResult<consultaMargemConsignavelResponse>(response,"",HttpStatusCode.OK);
                return apiResult;
            }
            catch (FaultException<ConsignacaoException> e)
            {
                return new ApiResult<consultaMargemConsignavelResponse?>(null,"",HttpStatusCode.BadRequest,
                    new List<DomainMessage> {
                        new DomainMessage(
                            e.Detail.code.ToString(),
                        HelperFromMessageExceptionBpo.ErroMessageConsultMargin(e.Message)) });

            }
        }

        public async Task<consultaMargemConsignavelResponse?> ConsultMarginConsignableByEnrollment(string idCovenant,
            string enrollment)
        {
            consultaMargemConsignavelRequest consultaMargemConsignavelRequest = new consultaMargemConsignavelRequest()
            {
                convenio = $"{idCovenant}",
                cpf = "?",
                matricula = enrollment,
                tipoOperacao = 1,
                nomeBanco = NOME_BANCO,
                usuarioAcesso = _bpoOption.Value.User,
                senhaAcesso = _bpoOption.Value.Password
            };
            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
            return await client.consultaMargemConsignavelAsync(consultaMargemConsignavelRequest);
        }

        public async Task<incluiEmprestimoResponse?> EndosamentMargin(incluiEmprestimoRequest incluiEmprestimo,
            string identifierInEndoser)
        {
            incluiEmprestimo.convenio = identifierInEndoser;
            incluiEmprestimo.nomeBanco = BankName.BankArbi;
            incluiEmprestimo.tipoOperacao = (int) OperationType.NewLoan;
            incluiEmprestimo.usuarioAcesso = _bpoOption.Value.User;
            incluiEmprestimo.senhaAcesso = _bpoOption.Value.Password;

            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);
            var resp = await client.incluiEmprestimoAsync(incluiEmprestimo);
            return resp;
        }

        public async Task<excluiEmprestimoResponse> CancelMarginReserve(excluiEmprestimoRequest excluiEmprestimo, string identifierInEndoser)
        {
            excluiEmprestimo.usuarioAcesso = _bpoOption.Value.User;
            excluiEmprestimo.senhaAcesso = _bpoOption.Value.Password;
            excluiEmprestimo.nomeBanco = BankName.BankArbi;
            excluiEmprestimo.convenio = identifierInEndoser;

            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);

            var resp = await client.excluiEmprestimoAsync(excluiEmprestimo);

            return resp;
        }

        public async Task<excluiEmprestimoResponse> CancelMarginEndorsament(excluiEmprestimoRequest excluiEmprestimo, string identifierInEndoser)
        {
            excluiEmprestimo.usuarioAcesso = _bpoOption.Value.User;
            excluiEmprestimo.senhaAcesso = _bpoOption.Value.Password;
            excluiEmprestimo.nomeBanco = BankName.BankArbi;
            excluiEmprestimo.convenio = identifierInEndoser;

            var client = new ConsignacaoClient(ConsignacaoClient.EndpointConfiguration.Consignacao,UrlBase);

            var resp = await client.excluiEmprestimoAsync(excluiEmprestimo);

            return resp;
        }
    }
}
