using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class MarginReserveStatusHistory : Entity<Guid>
    {
        public Guid MarginReserveId { get; protected set; }
        public MarginReserve MarginReserve { get; protected set; }
        public DateTime InsertDate { get; protected set; }
        public NormalizedString Messagem { get; protected set; }
        public MarginReserveStatus MarginReserveStatus { get; protected set; }

        protected MarginReserveStatusHistory() { }

        public MarginReserveStatusHistory(
            Guid marginReserveId,
            MarginReserveStatus status,
            string messagem
        )
        {
            MarginReserveId = marginReserveId;
            MarginReserveStatus = status;
            Messagem = messagem;
            InsertDate = DateTime.Now;
        }

        public MarginReserveStatusHistory(
            MarginReserveStatus status,
            string messagem
        )
        {
            MarginReserveStatus = status;
            Messagem = messagem;
            InsertDate = DateTime.Now;
        }
    }
}