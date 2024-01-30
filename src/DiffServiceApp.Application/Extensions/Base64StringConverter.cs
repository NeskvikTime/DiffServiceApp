namespace DiffServiceApp.Application.Extensions;
public static class Base64StringConverter
{
    public static byte[] FromBase64String(this string base64Data)
    {
        try
        {
            return Convert.FromBase64String(base64Data);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid base64 string");
        }
    }

    public static string ToBase64String(this byte[] data)
    {
        try
        {
            return Convert.ToBase64String(data);
        }
        catch (FormatException)
        {
            throw;
        }
    }
}
