namespace Jazz.Common;

public interface IHasAddressList
{
    IEnumerable<Address> Addresses { get; }
}