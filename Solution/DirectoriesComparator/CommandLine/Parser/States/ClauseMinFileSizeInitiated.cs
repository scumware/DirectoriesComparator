using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoriesComparator.CommandLine.Parser.States
{
    internal class ClauseMinFileSizeInitiated : ClauseInitiated
    {
        public override AbstractState ProcessWord(string p_Word, ParsingContext p_ParsingContext)
        {
            var errorState = new ErrorState();

            if (string.IsNullOrWhiteSpace(p_Word))
                return errorState;

            bool isDigitFound = false;

            int index = 0;
            var defaultErrorMessage = "Опция {0} не опознана";

            for (; index < p_Word.Length; index++)
            {
                var ch = p_Word[index];
                bool isDigit = char.IsDigit(ch);
                isDigitFound |= isDigit;

                if (false == isDigit)
                {
                    if (char.IsLetter(ch))
                        break;
                    else
                    {
                        p_ParsingContext.Parameters.AdditionalMessage = string.Format(defaultErrorMessage, p_Word);
                        return errorState;
                    }
                }
            }

            if (isDigitFound)
            {
                var dimension = SizeDimension.TryParse(p_Word, index);
                if (dimension == null)
                {
                    p_ParsingContext.Parameters.AdditionalMessage = string.Format(defaultErrorMessage, p_Word);
                    return errorState;
                }
                else
                {
                    p_ParsingContext.Parameters.MinFileSize = int.Parse(p_Word.Substring(0, index)) * dimension.Multiplier;
                    return InitialState.Item;
                }
            }

            p_ParsingContext.Parameters.AdditionalMessage = string.Format(defaultErrorMessage, p_Word);
            return errorState;
        }

        private object TryParseDimension(string p_word, int p_startIndex)
        {
            throw new NotImplementedException();
        }
    }
}
