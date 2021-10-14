using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BulkSenderSMTP.Services
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Helper method to populate list extracting string values between given characters
        /// </summary>
        public static List<string> EverythingBetween(this string source, string start, string end)
        {
            var results = new List<string>();

            string pattern = string.Format(
                "{0}({1}){2}",
                Regex.Escape(start),
                ".+?",
                 Regex.Escape(end));

            foreach (Match m in Regex.Matches(source, pattern))
            {
                results.Add(m.Groups[1].Value);
            }

            return results;
        }
    }
}
