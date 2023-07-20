using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BulkSenderSMTP.Services
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Helper method to populate list extracting string values between given characters
        /// </summary>
        public static IEnumerable<string> TakeStringInBetween(this string source, string start, string end)
        {
            if (source == null) throw new ArgumentNullException();

            var results = new List<string>();

            string pattern = SetStringPattern(start, end);

            foreach (Match m in Regex.Matches(source, pattern))
            {
                results.Add(m.Groups[1].Value);
            }

            return results;
        }

        private static string SetStringPattern(string start, string end)
        {
            return string.Format(
                "{0}({1}){2}",
                Regex.Escape(start),
                ".+?",
                 Regex.Escape(end));
        }
    }
}
