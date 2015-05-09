using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WITPJSON
{
    public class Game
    {
        public List<string> date_strs { get; set; }
        public Game(IEnumerable<Turn> turns, IEnumerable<UnitTimeline> timelines)
        {
            date_strs = turns.OrderBy(s => s.date).Select(s => s.date_string).Distinct().ToList();
        }
        public void Render()
        {
            if (!Directory.Exists(Program.output_timelines_directory))
                Directory.CreateDirectory(Program.output_timelines_directory);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(Path.Combine(Program.output_directory, "Game.json"), json);
        }
    }
}
