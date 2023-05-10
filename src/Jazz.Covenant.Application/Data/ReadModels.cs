using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Application.Data;

// TODO: Estratégia de versionamento, caso necessário
public static class ReadModels
{
    public class CovenantEndorser
    {
        public Guid EndoserId { get; set; }
        public string IdentifierInEndoser { get; set; }
        public EndoserAggregator EndoserIdentifier { get; set; }
    }

    public class EndosamentMargin
    {
        public StatusEndosament StatusEndosament { get; set; }
    }


    public class MarginReserveStatus
    {
        public Domain.Enums.MarginReserveStatus Status { get; set; }
    }
    public class Modality
    {
        public Guid ModalityId { get; set; }
        public string ModalityName { get; set; }
    }

    public class Covenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Organization { get; set; }
        public IList<string> Modality { get; set; } = new List<string>();
    }

    public class CovenantFavorite
    {
        public Guid Id { get; set; }
        public string TaxId { get; set; }
        public Guid CovenantId { get; set; }
        public bool Favorite { get; set; }
    }
}