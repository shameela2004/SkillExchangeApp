using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyApp1.Application.Utils
{
    public class SocialTextParser
    {
        private static readonly Regex MentionRegex = new Regex(@"@(\w+)", RegexOptions.Compiled);
        private static readonly Regex HashtagRegex = new Regex(@"#(\w+)", RegexOptions.Compiled);

        public (List<string> mentions, List<string> hashtags) ParseMentionsAndHashtags(string content)
        {
            var mentions = MentionRegex.Matches(content)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToList();

            var hashtags = HashtagRegex.Matches(content)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToList();

            return (mentions, hashtags);
        }
    }

}
