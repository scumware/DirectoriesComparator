using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.CommandLine.Parser
{
    public class SizeDimension
    {
        public const string KB = "kb";
        public const string MB = "mb";
        public const string GB = "gb";

        public readonly int Multiplier;

        public SizeDimension(int p_multiplier)
        {
            Multiplier = p_multiplier;
        }


        public static SizeDimension TryParse(string p_word, int p_startIndex)
        {
            var word = p_word.ToLower().Substring(p_startIndex).Trim();
            if (string.IsNullOrWhiteSpace(word))
                return null;

            if (word.StartsWith(KB))
                return new SizeDimension(1024);

            if (word.StartsWith(MB))
                return new SizeDimension(1024*1024);

            if (word.StartsWith(GB))
                return new SizeDimension(1024*1024*1024);

            return null;
        }
    }
}
