using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WoxEspanso
{
    public class Config
    {
        /// <summary>
        /// Load the Espanso configuration from the default path
        /// </summary>
        /// <returns>The Espanso config</returns>
        public static Config Load()
        {
            var document = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "espanso", "default.yml"));
            var input = new StringReader(document);
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            return deserializer.Deserialize<Config>(input);
        }

        public string ToggleKey { get; set; }
        public List<Match> Matches { get; set; }
    }

    public class Match
    {
        public string Trigger { get; set; }
        public string Replace { get; set; }
        public List<Var> Vars { get; set; }
    }

    public class Var
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Params { get; set; }
    }

}
