using System.Text.RegularExpressions;

namespace DiffServiceApp.Application.Extensions;

internal static class Base64StringValidator
{
    public static bool IsValidBase64String(this string base64String)
    {
        // Check if the string is null or empty
        if (string.IsNullOrEmpty(base64String))
        {
            return false;
        }

        // Check if length is a multiple of 4
        if (base64String.Length % 4 != 0)
        {
            return false;
        }

        // Regular expression to check if it contains only valid Base64 characters
        return Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }
}
