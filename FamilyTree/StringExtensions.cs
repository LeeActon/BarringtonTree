using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree
    {
    public static class StringExtensions
        {
        public static String StripCommentsAndExtraWhitespace(this String s, bool stripTrailingPunctuation = false)
            {
            StringBuilder builder = new StringBuilder();

            bool isComment = false;
            bool wasWhitespace = false;
            char endComment = '\0';
            for (int iSrc = 0; iSrc < s.Length; iSrc++)
                {
                Char ch = s[iSrc];
                bool isWhitespace = Char.IsWhiteSpace(ch);
                if (isComment)
                    {
                    // Skip it and check for end of comment marker.
                    if (ch == endComment)
                        {
                        isComment = false;
                        }
                    }
                else
                    {
                    if (ch == '(')
                        {
                        isComment = true;
                        endComment = ')';
                        }
                    else if (ch == '"')
                        {
                        isComment = true;
                        endComment = '"';
                        }
                    else
                        {
                        if (!isWhitespace && wasWhitespace)
                            {
                            if (builder.Length > 0)
                                {
                                builder.Append(' ');
                                }
                            }
                        wasWhitespace = isWhitespace;
                        if (!isWhitespace)
                            {
                            builder.Append(ch);
                            }
                        }
                    }
                }
            while (stripTrailingPunctuation)
                {
                int iLast = builder.Length - 1;
                Char lastChar = builder[iLast];
                if ((lastChar != '-') && Char.IsPunctuation(lastChar))
                    {
                    builder.Remove(iLast, 1);
                    }
                else
                    {
                    stripTrailingPunctuation = false;
                    }
                }

            return builder.ToString();
            }
        }
    }
