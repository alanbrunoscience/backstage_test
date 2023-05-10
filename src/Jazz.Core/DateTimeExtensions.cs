namespace Jazz.Core;

public static class DateTimeExtensions
{
    private static readonly DateTime ZeroTime = new DateTime(1, 1, 1);

    // TODO: Recuperar de serviço/base de dados durante a inicialização da aplicação
    private static readonly HashSet<DateTime> Holidays =
        new HashSet<DateTime>
        {
            DateTime.Parse("2022-01-01T00:00:00-03:00"),
            DateTime.Parse("2022-02-28T00:00:00-03:00"),
            DateTime.Parse("2022-03-01T00:00:00-03:00"),
            DateTime.Parse("2022-03-02T00:00:00-03:00"),
            DateTime.Parse("2022-04-15T00:00:00-03:00"),
            DateTime.Parse("2022-04-21T00:00:00-03:00"),
            DateTime.Parse("2022-05-01T00:00:00-03:00"),
            DateTime.Parse("2022-06-16T00:00:00-03:00"),
            DateTime.Parse("2022-09-07T00:00:00-03:00"),
            DateTime.Parse("2022-10-12T00:00:00-03:00"),
            DateTime.Parse("2022-11-15T00:00:00-03:00"),
            DateTime.Parse("2022-12-25T00:00:00-03:00"),
            DateTime.Parse("2022-12-30T00:00:00-03:00"),
            DateTime.Parse("2023-01-01T00:00:00-03:00"),
            DateTime.Parse("2023-02-21T00:00:00-03:00"),
            DateTime.Parse("2023-04-21T00:00:00-03:00"),
            DateTime.Parse("2023-05-01T00:00:00-03:00"),
            DateTime.Parse("2023-06-08T00:00:00-03:00"),
            DateTime.Parse("2023-09-07T00:00:00-03:00"),
            DateTime.Parse("2023-10-12T00:00:00-03:00"),
            DateTime.Parse("2023-11-15T00:00:00-03:00"),
            DateTime.Parse("2023-12-25T00:00:00-03:00")
        };

    private static bool IsHoliday(this DateTime date) => Holidays.Any(d => date >= d.GetStartfDay() && date <= d.GetEndOfDay());

    private static bool IsWeekend(this DateTime date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public static bool IsWorkingDay(this DateTime date) =>
        !date.IsHoliday()
        && date.DayOfWeek
            is DayOfWeek.Monday
            or DayOfWeek.Tuesday
            or DayOfWeek.Wednesday
            or DayOfWeek.Thursday
            or DayOfWeek.Friday;

    public static DateTime GetNextWorkingDay(this DateTime self)
    {
        var nextDay = self.AddDays(1);

        while (!nextDay.IsWorkingDay())
        {
            nextDay = nextDay.AddDays(1);
        }

        return nextDay.GetEndOfDay();
    }

    public static DateTime GetNextDayOfWeek(this DateTime self, DayOfWeek dayOfWeek)
    {
        var nextDay = self.AddDays(1);

        while (nextDay.DayOfWeek != dayOfWeek)
        {
            nextDay = nextDay.AddDays(1);
        }

        return nextDay.GetEndOfDay();
    }

    public static DateTime GetStartfDay(this DateTime self) => self.Date;

    public static DateTime GetEndOfDay(this DateTime self) => self.Date.Add(new TimeSpan(0, 23, 59, 59, 999));

    public static int GetElapsedYears(this DateTime self, DateTime other)
    {
        var span = self > other ? self - other : other - self;
        // Because we start at year 1 for the Gregorian
        // calendar, we must subtract a year here.
        var years = (ZeroTime + span).Year - 1;
        return years;
    }

    public static DateTime Normalize(this DateTime date) =>
        DateTime.Parse($"{date.Year:0000}-{date.Month:00}-{date.Day:00}T00:00:00.000-03:00").ToUniversalTime();
}