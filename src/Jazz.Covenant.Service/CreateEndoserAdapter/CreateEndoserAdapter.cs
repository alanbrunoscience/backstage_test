using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.Adapters;
using Jazz.Covenant.Service.Adapters.Mock;
using Jazz.Covenant.Service.BpoService;
using Jazz.Covenant.Service.PsaService;

namespace Jazz.Covenant.Service.CreateEndoserAdapter
{
    public class CreateEndoserAdapter : ICreateEndoserAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateEndoserAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEndoserAdapter CreateEndoser(Domain.Enums.EndoserAggregator endoserIdentifier)
        {
            if (endoserIdentifier == Domain.Enums.EndoserAggregator.PSA)
            {
                return new PsaAdpater(_serviceProvider.GetService(typeof(IPsaService)) as PsaService.PsaService);
            }
            else if (endoserIdentifier == Domain.Enums.EndoserAggregator.BPO)
            {
                return new BpoMockAdapter(_serviceProvider.GetService(typeof(IBpoService)) as BpoService.BpoService);
            }
            throw new Exception("Não existe essa averbadora");
        }
    }
}
