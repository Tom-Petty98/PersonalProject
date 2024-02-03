namespace PersonalProject.InternalPortal.Services.Helpers;

public static class DateTimeExtensions
{
    private static Lazy<TimeZoneInfo> _timeZoneForDisplay = new Lazy<TimeZoneInfo>(GetTimeZone);
    private static readonly TimeZoneInfo UkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

    public const string UiDateFormat = "d MMM yyyy";
    public const string UiDateTimeFormat = "d MMM yyyy HH:mm";

    //TODO: See if there's a way to config this.
    private static TimeZoneInfo GetTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }
        catch { /* ignore exception */}

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
        }
        catch { /* ignore exception */}

        return TimeZoneInfo.Utc;
    }

    public static DateTime ConvertUtcDateTimeToUk(this DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, UkTimeZone);
    }

    public static DateTime ConvertUtcDateTimeToLocal(this DateTime utcDateTime)
    {
        return utcDateTime.ToLocalTime();
    }

    public static string ToTimeZoneConvertedString(this DateTime dateTime, string? format = "dd MMMM yyyy HH:mm")
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, _timeZoneForDisplay.Value).ToString(format);
    }

    public static string? ToUiDateFormat(this DateTime? date)
    {
        return date?.ToString(UiDateFormat);
    }

    public static string? ToUiDateTimeFormat(this DateTime? date)
    {
        return date?.ToString(UiDateTimeFormat);
    }

    public static string ToUiDateTimeFormat(this DateTime date)
    {
        return date.ToString(UiDateTimeFormat);
    }
}
