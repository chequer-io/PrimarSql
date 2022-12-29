namespace PrimarSql.Common.Utilities;

public static class VerifyUtility
{
    public static T VerifyMember<T>(string? name, T? value) where T : class
    {
    }

    public static T VerifyNotNull<T>(string? name, T? value) where T : class
    {
        return value ?? throw new ArgumentNullException($"{name ?? "Unknown"} parameter must not be null.");
    }

    public static T VerifyNotNull<T>(string? name, T? value) where T : struct
    {
        return value ?? throw new ArgumentNullException($"{name ?? "Unknown"} parameter must not be null.");
    }
}
