using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerCncControl.Regexes
{
    public static class RegexHelper
    {
        const string blockComments = @"/\*(.*?)\*/";
        const string lineComments = @"//(.*?)\r?\n";

        public static string GetNoCommentString(string input)
        {
            string noComments = Regex.Replace(input + "\n", blockComments + "|" + lineComments,
                me => {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    return me.Value;
                }, RegexOptions.Singleline);

            return noComments;
        }
    }
}
