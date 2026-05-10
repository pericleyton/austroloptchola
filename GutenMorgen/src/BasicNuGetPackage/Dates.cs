namespace Eduq.Helpers;

public static class Dates
{
    public static DateTime AddDaysTo(this DateTime value, int days)
    {
        return value.AfterDays(days);
    }

    public static DateTime After(this DateTime value, TimeSpan duration)
    {
        return value.Add(duration);
    }

    public static DateTime AfterDays(this DateTime value, int days)
    {
        return value.AddDays(days);
    }

    public static DateTime AddBusinessDaysTo(this DateTime value, int days)
    {
        if (days == 0)
        {
            return value;
        }

        var direction = days > 0 ? 1 : -1;
        var remaining = Math.Abs(days);

        var current = value;
        while (remaining > 0)
        {
            current = current.AddDays(direction);
            if (current.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
            {
                remaining--;
            }
        }

        return current;
    }

    public static DateTime StartOfDayTo(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);
    }

    public static DateTime EndOfDayTo(this DateTime value)
    {
        return value.StartOfDayTo().AddDays(1).AddTicks(-1);
    }

    public static int DaysBetween(this DateTime a, DateTime b)
    {
        return (int)(b.StartOfDayTo() - a.StartOfDayTo()).TotalDays;
    }

    public static long ToUnixTimeSeconds(this DateTime value)
    {
        var dto = value.Kind == DateTimeKind.Unspecified
            ? new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc))
            : new DateTimeOffset(value);

        return dto.ToUniversalTime().ToUnixTimeSeconds();
    }

    public static DateTime FromUnixTimeSeconds(long unixSeconds, DateTimeKind kind = DateTimeKind.Utc)
    {
        var utc = DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime;

        return kind switch
        {
            DateTimeKind.Local => utc.ToLocalTime(),
            DateTimeKind.Utc => utc,
            _ => DateTime.SpecifyKind(utc, DateTimeKind.Unspecified)
        };
    }
}
