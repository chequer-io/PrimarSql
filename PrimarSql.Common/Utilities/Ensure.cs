namespace PrimarSql.Common.Utilities;

public static class Ensure
{
    public static T NotNull<T>(this T? value, string? name = null) where T : class
    {
        return value ?? throw new ArgumentNullException($"{name ?? "Unknown"} ({typeof(T).Name}) must not be null.");
    }

    public static T NotNull<T>(this T? value, string? name = null) where T : struct
    {
        return value ?? throw new ArgumentNullException($"{name ?? "Unknown"} ({typeof(T).Name}) must not be null.");
    }
}
