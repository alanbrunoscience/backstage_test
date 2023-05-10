using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class MarginEndosamentStatusHistory : Entity<Guid>
    {
        public MarginEndosamentStatusHistory(StatusEndosament statusEndosament, Guid endosamentMarginId,
            NormalizedString messagem)
        {
            StatusEndosament = statusEndosament;
            EndosamentMarginId = endosamentMarginId;
            Messagem = messagem;
            InsertDate = DateTime.Now;
        }

        public MarginEndosamentStatusHistory()
        {
        }

        public MarginEndosamentStatusHistory(StatusEndosament statusEndosament, EndosamentMargin endosamentMargin,
            Guid endosamentMarginId, NormalizedString messagem)
        {
            StatusEndosament = statusEndosament;
            EndosamentMargin = endosamentMargin;
            EndosamentMarginId = endosamentMarginId;
            InsertDate = DateTime.Now;
            Messagem = messagem;
        }

        public StatusEndosament StatusEndosament { get;  set; }
        public EndosamentMargin EndosamentMargin { get; protected set; }
        public Guid EndosamentMarginId { get; protected set; }
        public DateTime InsertDate { get; protected set; }
        public NormalizedString Messagem { get;  set; }
    }
}