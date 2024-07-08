using System.Text.Json;

namespace PersonalProject.InternalPortal.Services.Helpers;

public static class SessionExtensions
{
    public static void Put<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetOrDefault<T>(this ISession session, string key)
    {
        if (session.Keys.Any(k => k == key))
        {
            var stringValue = session.GetString(key);
            return string.IsNullOrEmpty(stringValue) ? default : JsonSerializer.Deserialize<T>(stringValue);
        }
        return default;
    }
}
