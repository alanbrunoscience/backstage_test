namespace Jazz.Common;

public record Address
{
    protected Address()
    {
        
    }
    
    private Address(NormalizedString street,
                    NormalizedString? number,
                    NormalizedString? complement,
                    NormalizedString? district,
                    NormalizedString state,
                    string postalCode,
                    NormalizedString? description)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Number = number;
        Complement = complement;
        District = district;
        State = state ?? throw new ArgumentNullException(nameof(state));
        PostalCode = PostalCode.From(postalCode);
        Description = description;
    }

    public NormalizedString Street { get; private set; }
    public NormalizedString? Number { get; private set; }
    public NormalizedString? Complement { get; private set; }
    public NormalizedString? District { get; private set; }
    public NormalizedString State { get; private set; }
    public PostalCode PostalCode { get; private set; }
    public NormalizedString? Description { get; private set; }

    public void Deconstruct(out NormalizedString street,
                            out NormalizedString? number,
                            out NormalizedString? complement,
                            out NormalizedString? district,
                            out NormalizedString state,
                            out PostalCode postalCode,
                            out NormalizedString? description)
    {
        street = Street;
        number = Number;
        complement = Complement;
        district = District;
        state = State;
        postalCode = PostalCode;
        description = Description;
    }

    public static Address From(NormalizedString street, NormalizedString? number, NormalizedString? complement, NormalizedString? district, NormalizedString state, NormalizedString postalCode, NormalizedString? description)
    {
        // TODO: Validação
        return new Address(street, number, complement, district, state, postalCode, description);
    }
}