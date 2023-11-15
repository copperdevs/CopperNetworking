using System.Text.Json;

namespace CopperNetworking.Common;

public static class Extensions
{
    public static int ConvertFpsToMilliseconds(this int i)
    {
        // takes fps and returns the distance between them
        if (i == 0) return 0;
        float milliseconds = 1f / i;
        milliseconds *= 1000;
        return (int)milliseconds;
    }
    
    public static byte[] ToByteArray(this string input)
    {
        return input.Select(Convert.ToByte).ToArray();
    }
    
    public static string ToString(this IEnumerable<byte> bytes)
    {
        return new string(bytes.Select(Convert.ToChar).ToArray());
    }
    
    public static string ToBase64String(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public static ArraySegment<byte> ToByteArraySegment(this object? obj)
    {
        return new ArraySegment<byte>(JsonSerializer.Serialize(obj).ToByteArray());
    }

    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}