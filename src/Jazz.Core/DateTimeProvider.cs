namespace Jazz.Core;

public static class DateTimeProvider
{
    public static DateTime Now => DateTimeProviderContext.Current == null
                                      ? DateTime.UtcNow
                                      : DateTimeProviderContext.Current.ContextDateTimeNow;

    public static DateTime UtcNow => Now;
}