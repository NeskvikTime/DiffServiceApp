using System.Buffers.Text;

namespace DiffServiceApp.Application.Extensions;

internal static class Base64StringValidator
{
    public static bool IsValidBase64String(this string base64String)
    {
        return Base64.IsValid(base64String);
    }
}
