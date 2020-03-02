using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using Wox.Plugin;
using Timer = System.Timers.Timer;

namespace WoxEspanso
{
    public class Main : IPlugin
    {
        private const string IconPath = "Assets\\icon.png";
        private Config config;
        private List<Match> matches;
        private Timer timer;

        public void Init(PluginInitContext context)
        {
            this.config = Config.Load();
            this.matches = this.config.Matches;
        }

        public List<Result> Query(Query query)
        {
            matches.Sort((match1, match2) =>
                fuzzySearch(query, match2).CompareTo(fuzzySearch(query, match1))
            );
            return matches.AsEnumerable().Select(match =>
            {
                var result = new Result
                {
                    Title = match.Trigger,
                    SubTitle = match.Replace,
                    IcoPath = IconPath,
                    Score = (int)Math.Ceiling(fuzzySearch(query, match) * 100),
                    Action = e =>
                    {
                        this.timer = new Timer(100);
                        this.timer.Elapsed += (source, ev) =>
                        {
                            new InputSimulator().Keyboard.TextEntry(match.Replace);
                        };
                        this.timer.AutoReset = false;
                        this.timer.Enabled = true;
                        return true;
                    }
                };
                return result;
            }).ToList();
        }

        private static double fuzzySearch(Query query, Match match)
        {
            if (query.Search.Length > 0 && query.Search[0] == '.')
            {
                var search = ":" + query.Search.Substring(1, query.Search.Length - 2);
                return match.Trigger.LevenshteinDistance(search);
            }
            var replace = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(match.Replace));
            return replace.DiceCoefficient(query.Search);
        }
    }
}