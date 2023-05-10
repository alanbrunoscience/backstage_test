
namespace Jazz.Covenant.Domain.Interfaces.Endoser
{
    public interface ICreateEndoserAdapter
    {
       IEndoserAdapter CreateEndoser(Enums.EndoserAggregator endoserIdentifier);
    }
}
