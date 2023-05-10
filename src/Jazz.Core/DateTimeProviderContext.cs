using System.Collections;

namespace Jazz.Core;

public class DateTimeProviderContext : IDisposable
{
    internal DateTime ContextDateTimeNow;
    private static readonly ThreadLocal<Stack> ThreadScopeStack = new ThreadLocal<Stack>(() => new Stack());

    public DateTimeProviderContext(DateTime contextDateTimeNow)
    {
        ContextDateTimeNow = contextDateTimeNow;
        ThreadScopeStack.Value?.Push(this);
    }

    public static DateTimeProviderContext? Current => 
        ThreadScopeStack.Value?.Count == 0 ? null : ThreadScopeStack.Value?.Peek() as DateTimeProviderContext;

    public void Dispose() => ThreadScopeStack.Value?.Pop();
}