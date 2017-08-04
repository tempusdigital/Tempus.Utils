namespace Tempus.Utils
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        // ToDo: adicionar teste unitário
        /// <summary>
        /// Formats the string according to the specified mask
        /// Fonte: <seealso cref="http://extensionmethod.net/csharp/string/formatwithmask"/>
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }

        public static string ToCamelCase(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var separatorsExp = @"([\.\[\]])";

            var separatorsParts = Regex.Split(source, separatorsExp).ToArray();

            var sb = new StringBuilder();

            var caseExp = @"[\s_-]|([A-Z]+[a-z]*)|([0-9])";

            foreach (var separatorPart in separatorsParts)
            {
                if (Regex.IsMatch(separatorPart, separatorsExp) || separatorPart == string.Empty)
                {
                    sb.Append(separatorPart);
                }
                else
                {
                    var sourceParts = Regex.Split(separatorPart, caseExp).Except(new[] { string.Empty }).ToArray();

                    sb.Append(sourceParts[0].ToLower());

                    foreach (var part in sourceParts.Skip(1))
                    {
                        var chars = part.ToLower().ToCharArray();
                        chars[0] = char.ToUpper(chars[0]);
                        sb.Append(chars);
                    }
                }
            }

            return sb.ToString();
        }

        // ToDo: adicionar teste unitário
        public static string ToTitleCase(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var exp = @"[\s_-]|([A-Z]+[a-z\.\[\]]*)";

            var sourceParts = Regex.Split(source, exp).Except(new[] { string.Empty }).ToArray();

            if (sourceParts.Length == 0)
                return source;

            var sb = new StringBuilder();

            foreach (var part in sourceParts)
            {
                var chars = part.ToLower().ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                sb.Append(chars);
            }

            return sb.ToString();
        }

        /// <summary>
        /// <see cref="string.IsNullOrWhiteSpace(string)"/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Remove os caracteres não numéricos        
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string OnlyNumbers(this string source)
        {
            // Fonte: http://stackoverflow.com/questions/3977497/stripping-out-non-numeric-characters-in-string
            return new string(source.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}
