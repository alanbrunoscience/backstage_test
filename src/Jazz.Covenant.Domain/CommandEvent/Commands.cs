

using Jazz.Core;

namespace Jazz.Covenant.Domain.CommandEvent;

public class Commands
{
  public record CancelMarginEndorsement(Guid IdMarginEndorsament) : DomainEvent;

  public record CancelReservationMargin(Guid IdMarginReserve) : DomainEvent;
}
