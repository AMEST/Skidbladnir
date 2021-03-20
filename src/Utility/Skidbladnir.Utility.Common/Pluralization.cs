using System;
using System.Collections.Generic;
using System.Linq;

namespace Skidbladnir.Utility.Common
{
    public static class Pluralization
    {
        private static readonly Dictionary<string, string> PluralExceptions = new Dictionary<string, string>()
        {
            {"man", "men"},
            {"woman", "women"},
            {"child", "children"},
            {"tooth", "teeth"},
            {"foot", "feet"},
            {"mouse", "mice"},
            {"belief", "beliefs"}
        };

        private static readonly string[] VoweltLetter = { "y", "ay", "ey", "iy", "oy", "uy" };
        private static readonly string[] ConsonantLetter = { "x", "ch", "sh", "us", "ss" };

        /// <summary>
        /// Attempts to pluralize the specified text according to the rules of the English language.
        /// Original code here: https://gist.github.com/andrewjk/3186582
        /// </summary>
        /// / <remarks>
        /// This function attempts to pluralize as many words as practical by following these rules:
        /// <list type="bullet">
        ///		<item><description>Words that don't follow any rules (e.g. "mouse" becomes "mice") are returned from a dictionary.</description></item>
        ///		<item><description>Words that end with "y" (but not with a vowel preceding the y) are pluralized by replacing the "y" with "ies".</description></item>
        ///		<item><description>Words that end with "us", "ss", "x", "ch" or "sh" are pluralized by adding "es" to the end of the text.</description></item>
        ///		<item><description>In all other cases, the translation will be by adding an "s" to the end of the word.</description></item>
        ///	</list>
        /// </remarks>
        public static string Plural(this string text)
        {
            if (PluralExceptions.ContainsKey(text.ToLower()))
                return PluralExceptions[text.ToLower()];

            if (VoweltLetter.Any(end => text.EndsWith(end, StringComparison.OrdinalIgnoreCase)))
                return text.Substring(0, text.Length - 1) + "ies";

            if (text.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                return text;

            if (ConsonantLetter.Any(end => text.EndsWith(end, StringComparison.OrdinalIgnoreCase)))
                return text + "es";

            return text + "s";
        }
    }
}