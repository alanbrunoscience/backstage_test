using Flurl.Http;
using Flurl.Http.Configuration;
using Jazz.Covenant.Service.CacheMemory;
using Jazz.Covenant.Service.PsaService.Dto.Autentication;
using Microsoft.Extensions.Configuration;
using Serilog;


namespace Jazz.Covenant.Service.PsaService.Autentication
{
    public class PsaAutentication : IPsaAutentication
    {
        private readonly IFlurlClient _flurlClient;
        private readonly IConfiguration _configuration;
        private readonly ICacheMemoryService _cacheMemoryService;
        private static readonly ILogger Log = Serilog.Log.ForContext<PsaAutentication>();

        public PsaAutentication(IFlurlClientFactory flurlClientFac, IConfiguration configuration, ICacheMemoryService cacheMemoryService)
        {
            _configuration = configuration;
            _flurlClient = flurlClientFac.Get(_configuration["Psa:base_url"]);
            _cacheMemoryService = cacheMemoryService;
           
        }



        public async Task<string> GetToken(int idCovenant)
        {

            var tokenCache = await _cacheMemoryService.Get(idCovenant);
            if (tokenCache is not null)
            {
                    return tokenCache.ToString();

            }

            return await GetTokenPsa(idCovenant);



        }
        private async Task<string> GetTokenPsa(int idCovenant)
        {
            try
            {
                var tokenPsa = await _flurlClient
                  .Request("/usuario/validar/")
                  .AllowHttpStatus("400,401,403,404")
                  .PostJsonAsync(new Login(idCovenant, _configuration["Psa:user"], _configuration["Psa:password"]))
                  .ReceiveJson<ResponseLogin>();
                _cacheMemoryService.Set(idCovenant, tokenPsa.Authorization,5);
                return tokenPsa.Authorization;
                
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Log.Error(ex, $"Error na requisição ao logar:{error}");
                throw;

            }



        }
    }
}
