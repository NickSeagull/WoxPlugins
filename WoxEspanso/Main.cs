using System;
using System.Collections.Generic;
using System.IO;
using Wox.Plugin;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WoxEspanso
{
    public class Main : IPlugin
    {
        private class Config
        {
            public string ToggleKey { get; set; }
            public List<Match> Matches { get; set; }
        }

        private class Match
        {
            public string Trigger { get; set; }
            public string Replace { get; set; }
        }

        public void Init(PluginInitContext context)
        {
        }

        public List<Result> Query(Query query)
        {
            var document = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "espanso", "default.yml"));
            var input = new StringReader(document);
            var deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var config = deserializer.Deserialize<Config>(input);
            var results = new List<Result>();
            results.Add(new Result()
            {
                Title = "Title",
                SubTitle = "Sub title",
                IcoPath = "Assets\\icon.png",  //relative path to your plugin directory
                Action = e =>
                {
                    // after user select the item

                    // return false to tell Wox don't hide query window, otherwise Wox will hide it automatically
                    return true;
                }
            });
            return results;
        }
    }

}
