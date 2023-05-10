using Flurl.Http;
using Flurl.Http.Configuration;
using Jazz.Covenant.Service.PsaService.Autentication;
using Jazz.Covenant.Service.PsaService.Dto.ConsultMargin;
using Jazz.Covenant.Service.PsaService.Dto.ConsultEmployee;
using Jazz.Covenant.Service.PsaService.Dto.ReservationConfirmation;
using Jazz.Covenant.Service.PsaService.Dto.MarginReserve;
using Microsoft.Extensions.Configuration;
using Serilog;
using Jazz.Covenant.Service.PsaService.Dto.Contract;
using Jazz.Covenant.Service.PsaService.Dto.DiscountCode;

namespace Jazz.Covenant.Service.PsaService
{
    public class PsaService : IPsaService
    {
        private readonly IPsaAutentication _psaAutentication;
        private readonly IFlurlClient _flurlClient;
        private static readonly ILogger Log = Serilog.Log.ForContext<PsaService>();
        public PsaService(IPsaAutentication psaAutentication, IFlurlClientFactory flurlClientFactory,IConfiguration configuration)
        {
            _psaAutentication = psaAutentication;
            _flurlClient = flurlClientFactory.Get(configuration["Psa:base_url"]) ;
        }
        public async Task<IEnumerable<Employee>> ConsultEmployeeByCpf(int idCovenant, string cpf)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request("/servidor/dadosFuncionais/" + cpf)
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<Employee>>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição consulta matricula:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<IEnumerable<Employee>> ConsultEmployeeByEnrollment(int idCovenant, string enrollment)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request("/servidor/dadosFuncionaisPorMatricula/" + enrollment)
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<Employee>>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição consulta matricula:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<IEnumerable<Margin>> ConsultMargin(int idCovenant, string enrollment)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request($"margem/consultaMargem/{enrollment}/senhaColaborador")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<Margin>>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição consulta margem:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<ResponseMarginReserve> MarginReserve(int idCovenant, RequestMarginReserve requestMarginReserve)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request("/contrato/reservaMargem")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .PostJsonAsync(requestMarginReserve)
                    .ReceiveJson<ResponseMarginReserve>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição reserva margem:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<ResponseReservationConfirmation> ReservationConfirmation(int idCovenant, RequestReservationConfirmation requestReservationConfirmation)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request("/contrato/confirmarReserva")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .PostJsonAsync(requestReservationConfirmation)
                    .ReceiveJson<ResponseReservationConfirmation>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição reserva margem:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<IEnumerable<Contract>> ListContract(int idCovenant, string enrollment)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request($"/contrato/listarContratos/{enrollment}")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<Contract>>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição listagem de contrato:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<ResponseSettleContract> SettleContract(int idCovenant, RequestSettleContract requestSettleContract)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request("/contrato/liquidar")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .PostJsonAsync(requestSettleContract)
                    .ReceiveJson<ResponseSettleContract>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição de liquidação de contrato:{error}");
                throw new Exception(error.ToString());
            }
        }
        public async Task<IEnumerable<ResponseDiscountCode>> GetDiscountCode(int idCovenant, string enrollment, int modality)
        {
            try
            {
                var token = await _psaAutentication.GetToken(idCovenant);
                return await _flurlClient.Request($"/codigoDesconto/codigosDescontoDisponivel/{enrollment}/{modality}")
                    .AllowHttpStatus("400,401,403")
                    .WithHeader("Authorization", token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<ResponseDiscountCode>>();
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição de buscar codigo de desconto:{error}");
                throw new Exception(error.ToString());
            }
        }
    }
}