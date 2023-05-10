using Jazz.Core;

namespace Jazz.Covenant.Domain.CommandEvent;

public static class Events
{
    public record CovenantRegistered(Guid CovenantId, string Name) : DomainEvent;

    public record CovenantNameUpdated(Guid CovenantId, string Name) : DomainEvent;

    public record CovenantStatusChanged(Guid CovenantId, string Status) : DomainEvent;


    public record CovenantMarginListed(Guid ConvenantId,  string Cpf,  string Enrollment,  string TypeProduct,
        DateTime DateTime) : DomainEvent;

    public record CovenantMarginReserved(Guid IdMarginReserve)  : DomainEvent;

    public record CovenantMarginEndosamented(Guid IdMarginEndorsament) :  DomainEvent;

}
