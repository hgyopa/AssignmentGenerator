using System;

namespace TestGenerator.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtension
    {
        public static string CaseInsensitiveReplace(this String str, string searchedText, string replacement)
        {
            var regex = new Regex(searchedText, RegexOptions.IgnoreCase);
            return regex.Replace(str, replacement);
        }
    }
}
