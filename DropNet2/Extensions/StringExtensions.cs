using System;

namespace DropNet2.Extensions
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string input)
        {
            return Uri.UnescapeDataString(input);
        }

        public static string UrlEncode(this string input)
        {
            return Uri.EscapeDataString(input);
        }

    }
}
