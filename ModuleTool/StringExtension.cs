using System.Text.RegularExpressions;

namespace ModuleTool
{
    public static class StringExtension
    {
        private static Regex PascalCasePattern =
            new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");

        public static string ToSnakeCase(this string str)
        {
            return string.Join("_", PascalCasePattern.Matches(str)).ToLower();
        }
    }
}
