namespace Jazz.Core;

public interface IAggregateSavedHandler
{
    Task HandleAggregateSaved(object aggregate);
}