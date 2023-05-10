namespace Jazz.Core;

public abstract record EntityId<TId> where TId : IEquatable<TId>
{
    protected EntityId(TId value)
    {
        if (value.Equals(default))
            throw new ArgumentException($"Invalid ID value: {value}.", nameof(value));
        Value = value;
    }

    protected TId Value { get; }

    public sealed override string ToString() => Value.ToString() ?? string.Empty;

    protected TId GetValue() => Value;
}